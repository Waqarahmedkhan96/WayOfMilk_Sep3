package sep3.grpc;

import io.grpc.stub.StreamObserver;
import net.devh.boot.grpc.server.service.GrpcService;
import sep3.dto.ContainerDtos;
import sep3.service.interfaces.IContainerService;

// from repeatingMessageTypes.proto
import sep3.wayofmilk.grpc.Empty;
import sep3.wayofmilk.grpc.SentId;

// from container_service.proto
import sep3.wayofmilk.grpc.ContainerListReply;
import sep3.wayofmilk.grpc.ContainerMessage;
import sep3.wayofmilk.grpc.ContainerServiceGrpc;
import sep3.wayofmilk.grpc.CreateContainerRequest;
import sep3.wayofmilk.grpc.UpdateContainerRequest;

@GrpcService
public class ContainerServiceGrpcImpl extends ContainerServiceGrpc.ContainerServiceImplBase {

    private final IContainerService containerService;

    public ContainerServiceGrpcImpl(IContainerService containerService) {
        this.containerService = containerService;
    }

    // CREATE
    @Override
    public void createContainer(CreateContainerRequest request,
                                StreamObserver<ContainerMessage> responseObserver) {

        ContainerDtos.CreateContainerDto dto = new ContainerDtos.CreateContainerDto();
        dto.setCapacityL(request.getCapacityL());

        ContainerDtos.ContainerDto created = containerService.create(dto);

        ContainerMessage reply = toGrpc(created);

        responseObserver.onNext(reply);
        responseObserver.onCompleted();
    }

    // UPDATE
    @Override
    public void updateContainer(UpdateContainerRequest request,
                                StreamObserver<ContainerMessage> responseObserver) {

        ContainerDtos.UpdateContainerDto dto = new ContainerDtos.UpdateContainerDto();
        dto.setId(request.getId());
        dto.setCapacityL(request.getCapacityL());

        ContainerDtos.ContainerDto updated = containerService.update(dto);

        ContainerMessage reply = toGrpc(updated);

        responseObserver.onNext(reply);
        responseObserver.onCompleted();
    }

    // DELETE
    @Override
    public void deleteContainer(SentId request,
                                StreamObserver<Empty> responseObserver) {

        containerService.delete(request.getId());

        responseObserver.onNext(Empty.newBuilder().build());
        responseObserver.onCompleted();
    }

    // GET ONE
    @Override
    public void getContainer(SentId request,
                             StreamObserver<ContainerMessage> responseObserver) {

        ContainerDtos.ContainerDto dto = containerService.get(request.getId());

        ContainerMessage reply = toGrpc(dto);

        responseObserver.onNext(reply);
        responseObserver.onCompleted();
    }

    // GET ALL
    @Override
    public void getAllContainers(Empty request,
                                 StreamObserver<ContainerListReply> responseObserver) {

        ContainerDtos.ContainerListDto listDto = containerService.getAll();

        ContainerListReply.Builder builder = ContainerListReply.newBuilder();
        for (ContainerDtos.ContainerDto c : listDto.getContainers()) {
            builder.addContainers(toGrpc(c));
        }

        responseObserver.onNext(builder.build());
        responseObserver.onCompleted();
    }

    // Helper: map internal DTO -> gRPC message
    private ContainerMessage toGrpc(ContainerDtos.ContainerDto dto) {
        return ContainerMessage.newBuilder()
                .setId(dto.getId())
                .setCapacityL(dto.getCapacityL())
                .setOccupiedCapacityL(dto.getOccupiedCapacityL()) // send occupied
                .build();
    }
}
