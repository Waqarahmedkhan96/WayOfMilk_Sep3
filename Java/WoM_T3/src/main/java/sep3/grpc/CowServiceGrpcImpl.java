package sep3.grpc;
import net.devh.boot.grpc.server.service.GrpcService;
import sep3.dto.MilkDtos;
import sep3.dto.cowDTO.CowCreationDTO;
import sep3.dto.cowDTO.CowDataDTO;
import sep3.mapping.GrpcMapper;
// Import generated message classes:

import io.grpc.stub.StreamObserver;
import sep3.service.interfaces.ICowService;
import sep3.wayofmilk.grpc.*;

import java.util.List;

@GrpcService // Marks this as a gRPC service implementation
public class CowServiceGrpcImpl extends CowServiceGrpc.CowServiceImplBase
{

  private final ICowService coreService;

  public CowServiceGrpcImpl(ICowService coreService)
  {
    this.coreService = coreService;
  }

  //CREATE/POST
  @Override public void addCow(CowCreationRequest request,
      StreamObserver<CowData> responseObserver)
  {
    // Convert the gRPC Request message into the Spring DTO
    CowCreationDTO creationDto = GrpcMapper.convertCowProtoCreationToDto(request);
    //TODO make sure birthdate is required from the client side

    // Call the Core Business Service
    CowDataDTO createdDto = coreService.addCow(creationDto);

    // Convert the resulting CowDataDTO back into the gRPC response message (CowData)
    CowData responseData = GrpcMapper.convertCowDtoToProto(createdDto);

    // Send the response and complete the call
    responseObserver.onNext(responseData);
    responseObserver.onCompleted();
  }

  //READ/FIND/GET/VIEW
  @Override public void getAllCows(Empty request,
      StreamObserver<CowList> responseObserver)
  {
    List<CowDataDTO> dtos = coreService.getAllCows();

    // Map the DTOs (CowDataDTO) to the gRPC messages (CowData)
    CowList.Builder cowListBuilder = CowList.newBuilder();

    for (CowDataDTO dto : dtos)
    {
      //using mapper to convert DTO to proto
      cowListBuilder.addCows(GrpcMapper.convertCowDtoToProto(dto));
    }
    // Send the response and complete the call
    responseObserver.onNext(cowListBuilder.build());
    responseObserver.onCompleted();
  }

  @Override public void getCowById(CowIdRequest request, StreamObserver<CowData> responseObserver)
  {
    //get id from request
    long cowToFindId = request.getId();
    //call service
    CowDataDTO foundCow = coreService.getCowById(cowToFindId);
    //convert to proto
    CowData responseData = GrpcMapper.convertCowDtoToProto(foundCow);
    //send response
    responseObserver.onNext(responseData);
    responseObserver.onCompleted();
  }

  @Override
  public void getCowByRegNo(SentString regNo, StreamObserver<CowData> responseObserver)
  {
    CowDataDTO foundCow = coreService.getCowByRegNo(regNo.getValue());
    CowData responseData = GrpcMapper.convertCowDtoToProto(foundCow);
    responseObserver.onNext(responseData);
    responseObserver.onCompleted();
  }
@Override
  public void getCowsByDepartmentId(SentId departmentId, StreamObserver<CowList> responseObserver)
  {
    List<CowDataDTO> foundCows = coreService.getCowsByDepartmentId(departmentId.getId());
    CowList.Builder cowListBuilder = CowList.newBuilder();
    for (CowDataDTO dto : foundCows)
    {
      cowListBuilder.addCows(GrpcMapper.convertCowDtoToProto(dto));
    }
    responseObserver.onNext(cowListBuilder.build());
    responseObserver.onCompleted();
  }

  @Override public void getMilksByCowId(CowIdRequest request,
      StreamObserver<MilkListReply> responseObserver)
  {
    List<MilkDtos.MilkDto> milkList = coreService.getCowMilk(request.getId());
    MilkListReply.Builder milkListBuilder = MilkListReply.newBuilder();
    for (MilkDtos.MilkDto dto : milkList)
    {
      milkListBuilder.addMilk(GrpcMapper.convertMilkDtoToProto(dto));
      //currently unsure how to add the resisteredBy field to the milk proto without editing the dto
      //fix later if time allows
    }
    responseObserver.onNext(milkListBuilder.build());
    responseObserver.onCompleted();
  }

  //UPDATE

  @Override
  public void updateCow(CowUpdateRequest request, StreamObserver<CowData> responseObserver) {
    // Convert partial gRPC request -> DTO with nulls
    CowDataDTO changesToCow = GrpcMapper.convertCowProtoToDto(request.getCowData());

    // Call Service (which is designed for this DTO)
    CowDataDTO updatedDto = coreService.updateCow(changesToCow, request.getRequestedBy());

    // Convert full result DTO -> gRPC response
    CowData responseData = GrpcMapper.convertCowDtoToProto(updatedDto);

    responseObserver.onNext(responseData);
    responseObserver.onCompleted();
  }

  @Override
  public void updateCowsHealth(CowsHealthChangeRequest request, StreamObserver<CowList> responseObserver)
  {
    // Extract data from the request
    List<Long> cowsIds = request.getCowIdsList();
    boolean healthUpdate = request.getNewHealthStatus();
    long userId = request.getRequestedByUserId();

    // Call the service to update health status for multiple cows
    coreService.updateManyCowsHealth(cowsIds, healthUpdate, userId);

    CowList.Builder cowListBuilder = CowList.newBuilder();
    for (Long id : cowsIds) {
      CowDataDTO dto = coreService.getCowById(id);
      cowListBuilder.addCows(GrpcMapper.convertCowDtoToProto(dto));
    }
    // Send the response and complete the call
    responseObserver.onNext(cowListBuilder.build());
    responseObserver.onCompleted();
  }


  //DELETE

  @Override public void deleteCow(CowIdRequest request, StreamObserver<Empty> responseObserver)
  {
    //get the id from the request
    long cowToDeleteId = request.getId();
    //call the service
    coreService.deleteCow(cowToDeleteId);
    //send the response
    responseObserver.onNext(Empty.getDefaultInstance());
    responseObserver.onCompleted();

  }

}