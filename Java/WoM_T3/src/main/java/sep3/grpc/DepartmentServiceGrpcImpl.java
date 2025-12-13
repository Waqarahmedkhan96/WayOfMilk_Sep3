package sep3.grpc;

import io.grpc.stub.StreamObserver;
import net.devh.boot.grpc.server.service.GrpcService;
import sep3.dto.cowDTO.CowDataDTO;
import sep3.dto.transferRecordDTO.TransferRecordDataDTO;
import sep3.entity.DepartmentType;
import sep3.mapping.GrpcMapper;
import sep3.service.interfaces.IDepartmentService;
import sep3.dto.departmentDTO.DepartmentCreationDTO;
import sep3.dto.departmentDTO.DepartmentDataDTO;
import sep3.wayofmilk.grpc.*;

import java.util.List;

@GrpcService
public class DepartmentServiceGrpcImpl extends DepartmentServiceGrpc.DepartmentServiceImplBase
{
    private final IDepartmentService coreService;

    public DepartmentServiceGrpcImpl(IDepartmentService coreService)
    {
        this.coreService = coreService;
    }

    @Override
    public void addDepartment(DepartmentCreationRequest request,
                              StreamObserver<DepartmentData> responseObserver) {

        try {
            DepartmentType type = DepartmentType.valueOf(request.getType().toUpperCase());
            DepartmentCreationDTO dto = new DepartmentCreationDTO(type, request.getName());

            DepartmentDataDTO result = coreService.addDepartment(dto);
            DepartmentData proto = GrpcMapper.convertDepartmentDtoToProto(result);

            responseObserver.onNext(proto);
            responseObserver.onCompleted();
        } catch (Exception e) {
            e.printStackTrace();
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
        System.out.println(">> GET ALL DEPARTMENTS CALLED");

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
        System.out.println(">> GET DEPARTMENT BY ID CALLED");
    }

    @Override
    public void getDepartmentsByType(DepartmentTypeRequest request,
                                     StreamObserver<DepartmentList> responseObserver)
    {
        try {
            // proto ENUM → entity enum
            DepartmentType type = DepartmentType.valueOf(request.getType().toUpperCase());

            List<DepartmentDataDTO> dtos = coreService.getDepartmentsByType(type);

            DepartmentList.Builder builder = DepartmentList.newBuilder();
            for (DepartmentDataDTO dto : dtos) {
                builder.addDepartments(GrpcMapper.convertDepartmentDtoToProto(dto));
            }

            responseObserver.onNext(builder.build());
            responseObserver.onCompleted();
        }
        catch (Exception e) {
            responseObserver.onError(e);
        }
        System.out.println(">> GET DEPARTMENTS BY TYPE CALLED");
    }

    @Override
    public void getDepartmentByName(DepartmentNameRequest request,
                                    StreamObserver<DepartmentData> responseObserver)
    {
        try
        {
            DepartmentDataDTO dto = coreService.getDepartmentByName(request.getName());
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
    public void updateDepartment(DepartmentData request,
                                 StreamObserver<DepartmentData> responseObserver)
    {
        try
        {
            DepartmentDataDTO dtoToUpdate =
                    GrpcMapper.convertDepartmentProtoToDto(request);

            DepartmentDataDTO updatedDto = coreService.updateDepartment(dtoToUpdate);

            DepartmentData responseData =
                    GrpcMapper.convertDepartmentDtoToProto(updatedDto);

            responseObserver.onNext(responseData);
            responseObserver.onCompleted();
        }
        catch (Exception e)
        {
            responseObserver.onError(e);
        }
        System.out.println(">> UPDATE DEPARTMENT CALLED");
    }

    @Override
    public void deleteDepartment(DepartmentIdRequest request,
                                 StreamObserver<Empty> responseObserver)
    {
        try
        {
            coreService.deleteDepartment(request.getId());
            responseObserver.onNext(Empty.newBuilder().build());
            responseObserver.onCompleted();
        }
        catch (Exception e)
        {
            responseObserver.onError(e);
        }
    }

    @Override
    public void getCowsByDepartment(DepartmentIdRequest request,
                                    StreamObserver<CowList> responseObserver) {
        try {
            List<CowDataDTO> cows = coreService.getCowsByDepartment(request.getId());

            CowList.Builder b = CowList.newBuilder();
            cows.forEach(c -> b.addCows(GrpcMapper.convertCowDtoToProto(c)));

            responseObserver.onNext(b.build());
            responseObserver.onCompleted();
        } catch (Exception e) {
            e.printStackTrace();
            responseObserver.onError(e);
        }
    }

    @Override
    public void getTransferRecordsByDepartment(DepartmentIdRequest request,
                                               StreamObserver<TransferRecordList> responseObserver) {
        try {
            List<TransferRecordDataDTO> records =
                    coreService.getTransferRecordsByDepartment(request.getId());

            TransferRecordList.Builder b = TransferRecordList.newBuilder();
            records.forEach(r -> b.addRecords(GrpcMapper.convertTransferRecordDtoToProto(r)));

            responseObserver.onNext(b.build());
            responseObserver.onCompleted();
        } catch (Exception e) {
            e.printStackTrace();
            responseObserver.onError(e);
        }
    }




}
