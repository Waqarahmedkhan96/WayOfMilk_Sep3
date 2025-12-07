package sep3.grpc;

import io.grpc.stub.StreamObserver;
import net.devh.boot.grpc.server.service.GrpcService;
import sep3.mapping.GrpcMapper;
import sep3.service.interfaces.IDepartmentDataService;
import sep3.dto.departmentDTO.DepartmentCreationDTO;
import sep3.dto.departmentDTO.DepartmentDataDTO;
import sep3.wayofmilk.grpc.*;

import java.util.List;

@GrpcService
public class DepartmentServiceGrpcImpl extends DepartmentServiceGrpc.DepartmentServiceImplBase
{
    private final IDepartmentDataService coreService;

    public DepartmentServiceGrpcImpl(IDepartmentDataService coreService)
    {
        this.coreService = coreService;
    }

    @Override
    public void addDepartment(DepartmentCreationRequest request,
                              StreamObserver<DepartmentData> responseObserver)
    {
        try
        {
            DepartmentCreationDTO creationDto =
                    GrpcMapper.convertDepartmentProtoCreationToDto(request);

            DepartmentDataDTO createdDto = coreService.addDepartment(creationDto);

            DepartmentData responseData =
                    GrpcMapper.convertDepartmentDtoToProto(createdDto);

            responseObserver.onNext(responseData);
            responseObserver.onCompleted();
        }
        catch (Exception e)
        {
            responseObserver.onError(e);
        }
    }

    @Override
    public void getAllDepartments(Empty request,
                                  StreamObserver<DepartmentList> responseObserver)
    {
        try
        {
            List<DepartmentDataDTO> dtos = coreService.getAllDepartments();

            DepartmentList.Builder listBuilder = DepartmentList.newBuilder();
            for (DepartmentDataDTO dto : dtos)
            {
                listBuilder.addDepartments(GrpcMapper.convertDepartmentDtoToProto(dto));
            }

            responseObserver.onNext(listBuilder.build());
            responseObserver.onCompleted();
        }
        catch (Exception e)
        {
            responseObserver.onError(e);
        }
    }

    @Override
    public void getDepartmentById(DepartmentIdRequest request,
                                  StreamObserver<DepartmentData> responseObserver)
    {
        try
        {
            DepartmentDataDTO dto = coreService.getDepartmentById(request.getId());
            DepartmentData response = GrpcMapper.convertDepartmentDtoToProto(dto);
            responseObserver.onNext(response);
            responseObserver.onCompleted();
        }
        catch (Exception e)
        {
            responseObserver.onError(e);
        }
    }

    @Override
    public void getDepartmentByType(DepartmentTypeRequest request,
                                    StreamObserver<DepartmentData> responseObserver)
    {
        try
        {
            DepartmentDataDTO dto =
                    coreService.getDepartmentByType(
                            GrpcMapper.convertDepartmentTypeStringToEnum(request.getType()));

            DepartmentData response = GrpcMapper.convertDepartmentDtoToProto(dto);
            responseObserver.onNext(response);
            responseObserver.onCompleted();
        }
        catch (Exception e)
        {
            responseObserver.onError(e);
        }
    }
}
