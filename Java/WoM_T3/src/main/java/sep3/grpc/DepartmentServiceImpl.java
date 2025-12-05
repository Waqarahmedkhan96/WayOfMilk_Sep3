package sep3.grpc;

import io.grpc.stub.StreamObserver;
import net.devh.boot.grpc.server.service.GrpcService;
import org.springframework.stereotype.Service;
import sep3.RequestHandlers.IDepartmentDataService;
import sep3.dto.DepartmentDataDTO;
import sep3.entity.DepartmentType;
import sep3.wayofmilk.grpc.*;

import java.util.List;

@GrpcService //fixed from Service so spring can find it in the right context
public class DepartmentServiceImpl extends DepartmentServiceGrpc.DepartmentServiceImplBase
{
  private final IDepartmentDataService departmentDataService;

  public DepartmentServiceImpl(IDepartmentDataService departmentDataService)
  {
    this.departmentDataService = departmentDataService;
  }

  //put everything in try-catch blocks so we get error messages back to the client
  @Override public void getAllDepartments(Empty request, StreamObserver<DepartmentList> responseObserver)
  {
    try {
      List<DepartmentDataDTO> list = departmentDataService.getAllDepartments();

      DepartmentList.Builder builder = DepartmentList.newBuilder();
      for (DepartmentDataDTO dto : list) {
        builder.addDepartments(
            DepartmentData.newBuilder()
                .setId(dto.getId())
                .setType(dto.getType().name())
                .build()
        );
      }

      responseObserver.onNext(builder.build());
      responseObserver.onCompleted();
    } catch (Exception e) {
      responseObserver.onError(e);
    }
  }

  @Override public void addDepartment(DepartmentCreationRequest request,
      StreamObserver<DepartmentData> responseObserver)
  {
    try {
      // FIX: This will throw IllegalArgumentException if the string doesn't match the Enum exactly
      // e.g. "Quarantine" vs "QUARANTINE"
      DepartmentType type = DepartmentType.valueOf(request.getType().toUpperCase()); // .toUpperCase() helps robustness

      DepartmentDataDTO dto = departmentDataService.addDepartment(type);

      DepartmentData response = DepartmentData.newBuilder()
          .setId(dto.getId())
          .setType(dto.getType().name())
          .build();

      responseObserver.onNext(response);
      responseObserver.onCompleted();
    } catch (IllegalArgumentException e) {
      // Handle invalid enum types specifically
      responseObserver.onError(new RuntimeException("Invalid Department Type. Valid values are: QUARANTINE, MILKING"));
    } catch (Exception e) {
      responseObserver.onError(e);
    }
  }

  @Override public void getDepartmentById(DepartmentIdRequest request,
      StreamObserver<DepartmentData> responseObserver)
  {
    try {
      DepartmentDataDTO dto = departmentDataService.getDepartmentById(request.getId());

      DepartmentData response = DepartmentData.newBuilder()
          .setId(dto.getId())
          .setType(dto.getType().name())
          .build();

      responseObserver.onNext(response);
      responseObserver.onCompleted();
    } catch (Exception e) {
      responseObserver.onError(e);
    }
  }

  @Override public void getDepartmentByType(DepartmentTypeRequest request,
      StreamObserver<DepartmentData> responseObserver)
  {
    try {
      DepartmentType type = DepartmentType.valueOf(request.getType().toUpperCase());
      DepartmentDataDTO dto = departmentDataService.getDepartmentByType(type);

      DepartmentData response = DepartmentData.newBuilder()
          .setId(dto.getId())
          .setType(dto.getType().name())
          .build();

      responseObserver.onNext(response);
      responseObserver.onCompleted();
    } catch (Exception e) {
      responseObserver.onError(e);
    }
  }
}
