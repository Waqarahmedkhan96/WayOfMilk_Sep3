package sep3.grpc;

import io.grpc.Status;
import io.grpc.stub.StreamObserver;
import net.devh.boot.grpc.server.service.GrpcService;
import sep3.dto.MilkDtos;
import sep3.service.interfaces.IMilkService;

import sep3.wayofmilk.grpc.*;

import java.time.LocalDate;

@GrpcService
public class MilkServiceGrpcImpl extends MilkServiceGrpc.MilkServiceImplBase {

    private final IMilkService milkService;

    public MilkServiceGrpcImpl(IMilkService milkService) {
        this.milkService = milkService;
    }

    // ---------- CREATE ----------
    @Override
    public void createMilk(CreateMilkRequest request,
                           StreamObserver<MilkMessage> responseObserver) {

        try {
            MilkDtos.CreateMilkDto dto = new MilkDtos.CreateMilkDto();
            dto.setCowId(request.getCowId());
            dto.setContainerId(request.getContainerId());
            dto.setRegisteredByUserId(request.getRegisteredByUserId());
            if (!request.getDate().isEmpty())
                dto.setDate(LocalDate.parse(request.getDate()));

            dto.setVolumeL(request.getVolumeL());
            dto.setTestResult(mapToDomainEnum(request.getTestResult()));

            MilkMessage reply = toGrpc(milkService.create(dto));

            responseObserver.onNext(reply);
            responseObserver.onCompleted();
        } catch (Exception e) {
            e.printStackTrace();
            responseObserver.onError(
                    Status.INTERNAL.withDescription("Create failed: " + e.getMessage())
                            .withCause(e)
                            .asRuntimeException()
            );
        }
    }

    // ---------- UPDATE ----------
    @Override
    public void updateMilk(UpdateMilkRequest request,
                           StreamObserver<MilkMessage> responseObserver) {

        MilkDtos.UpdateMilkDto dto = new MilkDtos.UpdateMilkDto();

        dto.setId(request.getId());
        if (request.getContainerId() != 0)
            dto.setContainerId(request.getContainerId());

        if (!request.getDate().isEmpty())
            dto.setDate(LocalDate.parse(request.getDate()));

        dto.setVolumeL(request.getVolumeL());
        dto.setTestResult(mapToDomainEnum(request.getTestResult()));

        MilkMessage reply = toGrpc(milkService.update(dto));

        responseObserver.onNext(reply);
        responseObserver.onCompleted();
    }

    // ---------- APPROVE STORAGE ----------
    @Override
    public void approveMilkStorage(ApproveMilkStorageRequest request,
                                   StreamObserver<MilkMessage> responseObserver) {

        try {
            MilkDtos.ApproveMilkStorageDto dto = new MilkDtos.ApproveMilkStorageDto();
            dto.setId(request.getMilkId());
            dto.setApprovedByUserId(request.getApprovedByUserId());
            dto.setApprovedForStorage(request.getApprovedForStorage());

            MilkMessage reply = toGrpc(milkService.approveForStorage(dto));
            responseObserver.onNext(reply);
            responseObserver.onCompleted();

        } catch (Exception e) {
            e.printStackTrace();
            responseObserver.onError(
                    Status.INTERNAL.withDescription("Approve failed: " + e.getMessage())
                            .withCause(e)
                            .asRuntimeException()
            );
        }
    }

    // ---------- DELETE ----------
    @Override
    public void deleteMilk(SentId request,
                           StreamObserver<Empty> responseObserver) {

        milkService.delete(request.getId());
        responseObserver.onNext(Empty.newBuilder().build());
        responseObserver.onCompleted();
    }

    // ---------- GET ----------
    @Override
    public void getMilk(SentId request,
                        StreamObserver<MilkMessage> responseObserver) {

        MilkMessage reply = toGrpc(milkService.get(request.getId()));
        responseObserver.onNext(reply);
        responseObserver.onCompleted();
    }

    // ---------- GET ALL ----------
    @Override
    public void getAllMilk(Empty request,
                           StreamObserver<MilkListReply> responseObserver) {

        MilkListReply.Builder builder = MilkListReply.newBuilder();
        milkService.getAll()
                .getMilkRecords()
                .forEach(m -> builder.addMilk(toGrpc(m)));

        responseObserver.onNext(builder.build());
        responseObserver.onCompleted();
    }

    // ---------- GET BY CONTAINER ----------
    @Override
    public void getMilkByContainer(MilkByContainerQuery request,
                                   StreamObserver<MilkListReply> responseObserver) {

        MilkDtos.MilkByContainerQuery dto = new MilkDtos.MilkByContainerQuery();
        dto.setContainerId(request.getContainerId());

        MilkListReply.Builder builder = MilkListReply.newBuilder();
        milkService.getByContainer(dto)
                .getMilkRecords()
                .forEach(m -> builder.addMilk(toGrpc(m)));

        responseObserver.onNext(builder.build());
        responseObserver.onCompleted();
    }

    // ---------- MAPPING ----------
    private MilkMessage toGrpc(MilkDtos.MilkDto dto) {
        MilkMessage.Builder builder = MilkMessage.newBuilder()
                .setId(dto.getId())
                .setCowId(dto.getCowId())
                .setContainerId(dto.getContainerId())
                .setVolumeL(dto.getVolumeL())
                .setTestResult(mapFromDomainEnum(dto.getTestResult()))
                .setApprovedForStorage(dto.isApprovedForStorage());

        if (dto.getDate() != null)
            builder.setDate(dto.getDate().toString());

        return builder.build();
    }

    private sep3.entity.MilkTestResult mapToDomainEnum(MilkTestResultEnum grpcEnum) {
        return switch (grpcEnum) {
            case MILK_TEST_PASS -> sep3.entity.MilkTestResult.PASS;
            case MILK_TEST_FAIL -> sep3.entity.MilkTestResult.FAIL;
            default -> sep3.entity.MilkTestResult.UNKNOWN;
        };
    }

    private MilkTestResultEnum mapFromDomainEnum(sep3.entity.MilkTestResult domain) {
        if (domain == null) return MilkTestResultEnum.MILK_TEST_UNKNOWN;

        return switch (domain) {
            case PASS -> MilkTestResultEnum.MILK_TEST_PASS;
            case FAIL -> MilkTestResultEnum.MILK_TEST_FAIL;
            default -> MilkTestResultEnum.MILK_TEST_UNKNOWN;
        };
    }
}
