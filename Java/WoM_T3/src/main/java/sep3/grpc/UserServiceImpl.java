package sep3.grpc;

import io.grpc.stub.StreamObserver;
import net.devh.boot.grpc.server.service.GrpcService;
import sep3.Mapping.GrpcMapper;
import sep3.RequestHandlers.IUserDataService;
import sep3.dto.userDTO.UserCreationDTO;
import sep3.dto.userDTO.UserDataDTO;
import sep3.dto.userDTO.UserLoginDTO;
import sep3.wayofmilk.grpc.*;

import java.util.List;

@GrpcService
public class UserServiceImpl extends UserServiceGrpc.UserServiceImplBase {

  private final IUserDataService userService;

  public UserServiceImpl(IUserDataService userService) {
    this.userService = userService;
  }

  // CREATE
  @Override
  public void addUser(UserCreationRequest request, StreamObserver<UserData> responseObserver) {
    try {
      // 1. Map Proto -> DTO
      UserCreationDTO creationDTO = GrpcMapper.userCreationProtoToDto(request);

      // 2. Call Business Logic
      UserDataDTO createdUser = userService.registerUser(creationDTO);

      // 3. Map DTO -> Proto
      UserData response = GrpcMapper.convertUserDtoToProto(createdUser);

      // 4. Respond
      responseObserver.onNext(response);
      responseObserver.onCompleted();
    } catch (Exception e) {
      responseObserver.onError(e);
    }
  }

  // READ (Get By ID)
  @Override
  public void getUserById(sentId request, StreamObserver<UserData> responseObserver) {
    try {
      // Assuming 'sentId' message has a field 'id' -> getId()
      long id = request.getId();

      UserDataDTO foundUser = userService.getUserById(id);
      UserData response = GrpcMapper.convertUserDtoToProto(foundUser);

      responseObserver.onNext(response);
      responseObserver.onCompleted();
    } catch (Exception e) {
      responseObserver.onError(e);
    }
  }

  // READ (Get All)
  @Override
  public void getAllUsers(Empty request, StreamObserver<UserList> responseObserver) {
    try {
      List<UserDataDTO> users = userService.getAllUsers();

      UserList.Builder listBuilder = UserList.newBuilder();
      for (UserDataDTO userDto : users) {
        listBuilder.addUsers(GrpcMapper.convertUserDtoToProto(userDto));
      }

      responseObserver.onNext(listBuilder.build());
      responseObserver.onCompleted();
    } catch (Exception e) {
      responseObserver.onError(e);
    }
  }

  // READ (Get By Name)
  @Override
  public void getUsersByName(sentString request, StreamObserver<UserList> responseObserver) {
    try {
      String nameToSearch = request.getValue();

      List<UserDataDTO> users = userService.getUsersByName(nameToSearch);

      UserList.Builder listBuilder = UserList.newBuilder();
      for (UserDataDTO userDto : users) {
        listBuilder.addUsers(GrpcMapper.convertUserDtoToProto(userDto));
      }

      responseObserver.onNext(listBuilder.build());
      responseObserver.onCompleted();
    } catch (Exception e) {
      responseObserver.onError(e);
    }
  }

  // UPDATE
  @Override
  public void updateUser(UserData request, StreamObserver<UserData> responseObserver) {
    try {
      // Map partial update request to DTO
      UserDataDTO changesDto = GrpcMapper.convertUserProtoToDto(request);

      UserDataDTO updatedUser = userService.updateUser(changesDto);
      UserData response = GrpcMapper.convertUserDtoToProto(updatedUser);

      responseObserver.onNext(response);
      responseObserver.onCompleted();
    } catch (Exception e) {
      responseObserver.onError(e);
    }
  }

  // DELETE
  @Override
  public void deleteUser(sentId request, StreamObserver<Empty> responseObserver) {
    try {
      userService.deleteUser(request.getId());

      responseObserver.onNext(Empty.getDefaultInstance());
      responseObserver.onCompleted();
    } catch (Exception e) {
      responseObserver.onError(e);
    }
  }

  // AUTHENTICATE / VALIDATE
  @Override
  public void authenticateUser(AuthenticationRequest request, StreamObserver<UserData> responseObserver) {
    try {
      // 1. Map Proto (Email/Pass) -> LoginDTO
      UserLoginDTO loginDTO = GrpcMapper.convertLoginProtoToDto(request);

      // 2. Validate (Throws exception if invalid)
      UserDataDTO validatedUser = userService.validateUser(loginDTO);

      // 3. Return the full user data (ID, Role, etc.) so T2 can make a token
      UserData response = GrpcMapper.convertUserDtoToProto(validatedUser);

      responseObserver.onNext(response);
      responseObserver.onCompleted();
    } catch (Exception e) {
      // Returns a gRPC error to T2 (e.g., INVALID_ARGUMENT or UNKNOWN)
      responseObserver.onError(e);
    }
  }
}