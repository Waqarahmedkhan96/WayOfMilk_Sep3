package sep3.grpc;

import io.grpc.stub.StreamObserver;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.junit.jupiter.MockitoExtension;
import sep3.dto.userDTO.UserDataDTO;
import sep3.service.interfaces.IUserService;
import sep3.wayofmilk.grpc.AuthenticationRequest;
import sep3.wayofmilk.grpc.UserCreationRequest;
import sep3.wayofmilk.grpc.UserData;

import static org.mockito.ArgumentMatchers.any;
import static org.mockito.Mockito.*;

@ExtendWith(MockitoExtension.class)
class UserServiceGrpcImplTest {

    @Mock
    private IUserService userService;

    @InjectMocks
    private UserServiceGrpcImpl grpcService;

    @Test
    void addUser_validRequest_returnsUser() {
        UserDataDTO dto = new UserDataDTO();
        dto.setId(1L);

        when(userService.registerUser(any())).thenReturn(dto);

        UserCreationRequest request = UserCreationRequest.newBuilder().build();
        StreamObserver<UserData> observer = mock(StreamObserver.class);

        grpcService.addUser(request, observer);

        verify(observer).onNext(any(UserData.class));
        verify(observer).onCompleted();
    }

    @Test
    void authenticateUser_validCredentials_returnsUser() {
        UserDataDTO dto = new UserDataDTO();
        when(userService.validateUser(any())).thenReturn(dto);

        AuthenticationRequest request = AuthenticationRequest.newBuilder().build();
        StreamObserver<UserData> observer = mock(StreamObserver.class);

        grpcService.authenticateUser(request, observer);

        verify(observer).onNext(any(UserData.class));
        verify(observer).onCompleted();
    }


}
