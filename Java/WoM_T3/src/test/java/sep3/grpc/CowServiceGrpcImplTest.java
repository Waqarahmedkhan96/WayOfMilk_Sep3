package sep3.grpc;

import sep3.wayofmilk.grpc.Empty;
import io.grpc.stub.StreamObserver;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.junit.jupiter.MockitoExtension;
import sep3.dto.cowDTO.CowDataDTO;
import sep3.service.interfaces.ICowService;
import sep3.wayofmilk.grpc.CowData;
import sep3.wayofmilk.grpc.CowIdRequest;

import static org.mockito.Mockito.*;

@ExtendWith(MockitoExtension.class)
class CowServiceGrpcImplTest {

    @Mock
    private ICowService cowService;

    @InjectMocks
    private CowServiceGrpcImpl grpcService;

    @Test
    void getCowById_validId_returnsCow() {
        CowDataDTO dto = new CowDataDTO(1L, "COW-1", null, true, null, null);
        when(cowService.getCowById(1L)).thenReturn(dto);

        CowIdRequest request = CowIdRequest.newBuilder().setId(1L).build();
        StreamObserver<CowData> observer = mock(StreamObserver.class);

        grpcService.getCowById(request, observer);

        verify(observer).onNext(any(CowData.class));
        verify(observer).onCompleted();
    }

    @Test
    void deleteCow_callsServiceAndReturnsEmpty() {
        StreamObserver<Empty> observer = mock(StreamObserver.class);
        CowIdRequest request = CowIdRequest.newBuilder().setId(5L).build();

        grpcService.deleteCow(request, observer);

        verify(cowService).deleteCow(5L);
        verify(observer).onNext(Empty.getDefaultInstance());
        verify(observer).onCompleted();
    }
}
