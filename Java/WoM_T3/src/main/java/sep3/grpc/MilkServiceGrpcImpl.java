package sep3.grpc;

import io.grpc.stub.StreamObserver;
import net.devh.boot.grpc.server.service.GrpcService;
import sep3.dto.MilkDtos;
import sep3.service.interfaces.IMilkService;

// from repeatingMessageTypes.proto
import sep3.wayofmilk.grpc.Empty;
import sep3.wayofmilk.grpc.SentId;

// from milk_service.proto
import sep3.wayofmilk.grpc.MilkByContainerQuery;
import sep3.wayofmilk.grpc.MilkListReply;
import sep3.wayofmilk.grpc.MilkMessage;
import sep3.wayofmilk.grpc.MilkServiceGrpc;
import sep3.wayofmilk.grpc.CreateMilkRequest;
import sep3.wayofmilk.grpc.UpdateMilkRequest;
import sep3.wayofmilk.grpc.MilkTestResultEnum;

@GrpcService
public class MilkServiceGrpcImpl extends MilkServiceGrpc.MilkServiceImplBase {

    private final IMilkService milkService;

    public MilkServiceGrpcImpl(IMilkService milkService) {
        this.milkService = milkService;
    }

    // CREATE
    @Override
    public void createMilk(CreateMilkRequest request,
                           StreamObserver<MilkMessage> responseObserver) {

        MilkDtos.CreateMilkDto dto = new MilkDtos.CreateMilkDto();
        dto.setCowId(request.getCowId());
        dto.setContainerId(request.getContainerId());
        dto.setRegisteredByUserId(request.getRegisteredByUserId());

        // date: if empty string, let service / mapper default to today
        if (!request.getDate().isEmpty()) {
            dto.setDate(java.time.LocalDate.parse(request.getDate()));
        }

        dto.setVolumeL(request.getVolumeL());
        dto.setTestResult(mapToDomainEnum(request.getTestResult()));

        MilkDtos.MilkDto created = milkService.create(dto);

        MilkMessage reply = toGrpc(created);
        responseObserver.onNext(reply);
        responseObserver.onCompleted();
    }

    // UPDATE
    @Override
    public void updateMilk(UpdateMilkRequest request,
                           StreamObserver<MilkMessage> responseObserver) {

        MilkDtos.UpdateMilkDto dto = new MilkDtos.UpdateMilkDto();
        dto.setId(request.getId());

        // containerId: treat 0 as "no change" if that's how your DTO works
        if (request.getContainerId() != 0) {
            dto.setContainerId(request.getContainerId());
        }

        if (!request.getDate().isEmpty()) {
            dto.setDate(java.time.LocalDate.parse(request.getDate()));
        }

        dto.setVolumeL(request.getVolumeL());
        dto.setTestResult(mapToDomainEnum(request.getTestResult()));

        MilkDtos.MilkDto updated = milkService.update(dto);

        MilkMessage reply = toGrpc(updated);
        responseObserver.onNext(reply);
        responseObserver.onCompleted();
    }

    // DELETE
    @Override
    public void deleteMilk(SentId request,
                           StreamObserver<Empty> responseObserver) {

        milkService.delete(request.getId());

        responseObserver.onNext(Empty.newBuilder().build());
        responseObserver.onCompleted();
    }

    // GET ONE
    @Override
    public void getMilk(SentId request,
                        StreamObserver<MilkMessage> responseObserver) {

        MilkDtos.MilkDto dto = milkService.get(request.getId());

        MilkMessage reply = toGrpc(dto);
        responseObserver.onNext(reply);
        responseObserver.onCompleted();
    }

    // GET ALL
    @Override
    public void getAllMilk(Empty request,
                           StreamObserver<MilkListReply> responseObserver) {

        MilkDtos.MilkListDto listDto = milkService.getAll();

        MilkListReply.Builder builder = MilkListReply.newBuilder();
        listDto.getMilkRecords().forEach(m -> builder.addMilk(toGrpc(m)));

        responseObserver.onNext(builder.build());
        responseObserver.onCompleted();
    }

    // GET BY CONTAINER
    @Override
    public void getMilkByContainer(MilkByContainerQuery request,
                                   StreamObserver<MilkListReply> responseObserver) {

        MilkDtos.MilkByContainerQuery dto = new MilkDtos.MilkByContainerQuery();
        dto.setContainerId(request.getContainerId());

        MilkDtos.MilkListDto listDto = milkService.getByContainer(dto);

        MilkListReply.Builder builder = MilkListReply.newBuilder();
        listDto.getMilkRecords().forEach(m -> builder.addMilk(toGrpc(m)));

        responseObserver.onNext(builder.build());
        responseObserver.onCompleted();
    }

    // Helper: map internal DTO -> gRPC message
    private MilkMessage toGrpc(MilkDtos.MilkDto dto) {
        MilkMessage.Builder builder = MilkMessage.newBuilder()
                .setId(dto.getId())
                .setCowId(dto.getCowId())
                .setContainerId(dto.getContainerId())
                .setVolumeL(dto.getVolumeL())
                .setTestResult(mapFromDomainEnum(dto.getTestResult()));

        // MilkDto has no registeredByUserId, so we leave it as default 0

        if (dto.getDate() != null) {
            builder.setDate(dto.getDate().toString()); // yyyy-MM-dd
        }

        return builder.build();
    }

    // Map gRPC enum -> domain enum (MilkTestResult)
    private sep3.entity.MilkTestResult mapToDomainEnum(MilkTestResultEnum grpcEnum) {
        return switch (grpcEnum) {
            case MILK_TEST_PASS -> sep3.entity.MilkTestResult.PASS;
            case MILK_TEST_FAIL -> sep3.entity.MilkTestResult.FAIL;
            case MILK_TEST_UNKNOWN, UNRECOGNIZED -> sep3.entity.MilkTestResult.UNKNOWN;
        };
    }

    // Map domain enum -> gRPC enum
    private MilkTestResultEnum mapFromDomainEnum(sep3.entity.MilkTestResult domainEnum) {
        if (domainEnum == null) return MilkTestResultEnum.MILK_TEST_UNKNOWN;
        return switch (domainEnum) {
            case PASS -> MilkTestResultEnum.MILK_TEST_PASS;
            case FAIL -> MilkTestResultEnum.MILK_TEST_FAIL;
            case UNKNOWN -> MilkTestResultEnum.MILK_TEST_UNKNOWN;
        };
    }
}
