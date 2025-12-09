package sep3.grpc;

import io.grpc.stub.StreamObserver;
import net.devh.boot.grpc.server.service.GrpcService;
import sep3.mapping.GrpcMapper;
import sep3.service.interfaces.IUserService;
import sep3.dto.userDTO.UserCreationDTO;
import sep3.dto.userDTO.UserDataDTO;
import sep3.dto.userDTO.UserLoginDTO;
import sep3.wayofmilk.grpc.*;

import java.util.List;

@GrpcService
public class UserServiceGrpcImpl extends UserServiceGrpc.UserServiceImplBase {

  private final IUserService userService;

  public UserServiceGrpcImpl(IUserService userService) {
    this.userService = userService;
  }

  // CREATE
  @Override
  public void addUser(UserCreationRequest request, StreamObserver<UserData> responseObserver) {

      // 1. Map Proto -> DTO
      UserCreationDTO creationDTO = GrpcMapper.userCreationProtoToDto(request);

      // 2. Call Business Logic
      UserDataDTO createdUser = userService.registerUser(creationDTO);

      // 3. Map DTO -> Proto
      UserData response = GrpcMapper.convertUserDtoToProto(createdUser);

      // 4. Respond
      responseObserver.onNext(response);
      responseObserver.onCompleted();

  }

  // READ (Get By ID)
  @Override
  public void getUserById(SentId request, StreamObserver<UserData> responseObserver) {

      // Assuming 'sentId' message has a field 'id' -> getId()
      long id = request.getId();

      UserDataDTO foundUser = userService.getUserById(id);
      UserData response = GrpcMapper.convertUserDtoToProto(foundUser);

      responseObserver.onNext(response);
      responseObserver.onCompleted();

  }

  // READ (Get All)
  @Override
  public void getAllUsers(Empty request, StreamObserver<UserList> responseObserver) {

      List<UserDataDTO> users = userService.getAllUsers();

      UserList.Builder listBuilder = UserList.newBuilder();
      for (UserDataDTO userDto : users) {
        listBuilder.addUsers(GrpcMapper.convertUserDtoToProto(userDto));
      }

      responseObserver.onNext(listBuilder.build());
      responseObserver.onCompleted();

  }

  // READ (Get By Name)
  @Override
  public void getUsersByName(SentString request, StreamObserver<UserList> responseObserver) {

      String nameToSearch = request.getValue();

      List<UserDataDTO> users = userService.getUsersByName(nameToSearch);

      UserList.Builder listBuilder = UserList.newBuilder();
      for (UserDataDTO userDto : users) {
        listBuilder.addUsers(GrpcMapper.convertUserDtoToProto(userDto));
      }

      responseObserver.onNext(listBuilder.build());
      responseObserver.onCompleted();

  }

  // UPDATE
  @Override
  public void updateUser(UserData request, StreamObserver<UserData> responseObserver) {

      // Map partial update request to DTO
      UserDataDTO changesDto = GrpcMapper.convertUserProtoToDto(request);

      UserDataDTO updatedUser = userService.updateUser(changesDto);
      UserData response = GrpcMapper.convertUserDtoToProto(updatedUser);

      responseObserver.onNext(response);
      responseObserver.onCompleted();

  }

  // DELETE
  @Override
  public void deleteUser(SentId request, StreamObserver<Empty> responseObserver) {

      userService.deleteUser(request.getId());

      responseObserver.onNext(Empty.getDefaultInstance());
      responseObserver.onCompleted();
  }

  // AUTHENTICATE / VALIDATE
  @Override
  public void authenticateUser(AuthenticationRequest request, StreamObserver<UserData> responseObserver) {

      // 1. Map Proto (Email/Pass) -> LoginDTO
      UserLoginDTO loginDTO = GrpcMapper.convertLoginProtoToDto(request);

      // 2. Validate (Throws exception if invalid)
      UserDataDTO validatedUser = userService.validateUser(loginDTO);

      // 3. Return the full user data (ID, Role, etc.) so T2 can make a token
      UserData response = GrpcMapper.convertUserDtoToProto(validatedUser);

      responseObserver.onNext(response);
      responseObserver.onCompleted();
  }

  @Override
  public void changePassword(ChangePasswordRequest request, StreamObserver<SentBool> responseObserver) {

    long userId = request.getId();
    String oldPassword = request.getOldPassword();
    String newPassword = request.getNewPassword();

    userService.changePassword(oldPassword, newPassword, userId);

    responseObserver.onNext(SentBool.newBuilder().setValue(true).build());
    responseObserver.onCompleted();
  }

  @Override public void resetPassword(SentId request, StreamObserver<Empty> responseObserver)
  {
    userService.resetPassword(request.getId());

    responseObserver.onNext(Empty.getDefaultInstance());
    responseObserver.onCompleted();
  }
}