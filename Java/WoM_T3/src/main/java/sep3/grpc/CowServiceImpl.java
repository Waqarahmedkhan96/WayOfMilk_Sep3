package sep3.grpc;
import net.devh.boot.grpc.server.service.GrpcService;
import sep3.dto.CowCreationDTO;
import sep3.dto.CowDataDTO;
import sep3.Mapping.GrpcMapper;
// Import generated message classes:

import io.grpc.stub.StreamObserver;
import sep3.RequestHandlers.ICowDataService;
import sep3.wayofmilk.grpc.*;

import java.util.List;

@GrpcService // Marks this as a gRPC service implementation
public class CowServiceImpl extends CowServiceGrpc.CowServiceImplBase
{

  private final ICowDataService coreService;

  public CowServiceImpl(ICowDataService coreService)
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

  //UPDATE

  @Override
  public void updateCow(CowData request, StreamObserver<CowData> responseObserver) {
    // Convert partial gRPC request -> DTO with nulls
    CowDataDTO changesToCow = GrpcMapper.convertCowProtoToDto(request);

    // Call Service (which is designed for this DTO)
    CowDataDTO updatedDto = coreService.updateCow(changesToCow);

    // Convert full result DTO -> gRPC response
    CowData responseData = GrpcMapper.convertCowDtoToProto(updatedDto);

    responseObserver.onNext(responseData);
    responseObserver.onCompleted();
  }

  public void updateCowHealth(CowData request, StreamObserver<CowData> responseObserver)
  {
    // Convert partial gRPC request -> DTO with nulls
    CowDataDTO changesToCow = GrpcMapper.convertCowProtoToDto(request);

    // Call Service (which is designed for this DTO)
    CowDataDTO updatedDto = coreService.updateCowHealth(changesToCow);

    // Convert full result DTO -> gRPC response
    CowData responseData = GrpcMapper.convertCowDtoToProto(updatedDto);

    responseObserver.onNext(responseData);
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