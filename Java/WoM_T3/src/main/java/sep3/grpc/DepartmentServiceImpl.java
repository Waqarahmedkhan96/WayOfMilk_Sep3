package sep3.grpc;

import io.grpc.stub.StreamObserver;
import org.springframework.stereotype.Service;
import sep3.RequestHandlers.IDepartmentDataService;
import sep3.dto.DepartmentDataDTO;
import sep3.entity.DepartmentType;
import sep3.wayofmilk.grpc.*;

import java.util.List;

@Service
public class DepartmentServiceImpl extends DepartmentServiceGrpc.DepartmentServiceImplBase
{
  private final IDepartmentDataService departmentDataService;

  public DepartmentServiceImpl(IDepartmentDataService departmentDataService)
  {
    this.departmentDataService = departmentDataService;
  }

  @Override
  public void getAllDepartments(Empty request, StreamObserver<DepartmentList> responseObserver) {
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
  }

  @Override
  public void addDepartment(DepartmentCreationRequest request, StreamObserver<DepartmentData> responseObserver) {
    DepartmentType type = DepartmentType.valueOf(request.getType());
    DepartmentDataDTO dto = departmentDataService.addDepartment(type);

    DepartmentData response = DepartmentData.newBuilder()
        .setId(dto.getId())
        .setType(dto.getType().name())
        .build();

    responseObserver.onNext(response);
    responseObserver.onCompleted();
  }

  @Override
  public void getDepartmentById(DepartmentIdRequest request, StreamObserver<DepartmentData> responseObserver) {
    DepartmentDataDTO dto = departmentDataService.getDepartmentById(request.getId());

    DepartmentData response = DepartmentData.newBuilder()
        .setId(dto.getId())
        .setType(dto.getType().name())
        .build();

    responseObserver.onNext(response);
    responseObserver.onCompleted();
  }

  @Override
  public void getDepartmentByType(DepartmentTypeRequest request, StreamObserver<DepartmentData> responseObserver) {
    DepartmentType type = DepartmentType.valueOf(request.getType());
    DepartmentDataDTO dto = departmentDataService.getDepartmentByType(type);

    DepartmentData response = DepartmentData.newBuilder()
        .setId(dto.getId())
        .setType(dto.getType().name())
        .build();

    responseObserver.onNext(response);
    responseObserver.onCompleted();
  }
}
