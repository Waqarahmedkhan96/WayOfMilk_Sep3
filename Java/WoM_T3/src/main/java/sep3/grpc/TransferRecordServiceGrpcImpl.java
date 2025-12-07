package sep3.grpc;

import io.grpc.stub.StreamObserver;
import net.devh.boot.grpc.server.service.GrpcService;

import sep3.mapping.GrpcMapper;
import sep3.service.interfaces.ITransferRecordService;
import sep3.dto.transferRecordDTO.TransferRecordCreationDTO;
import sep3.dto.transferRecordDTO.TransferRecordDataDTO;

import sep3.wayofmilk.grpc.*;

import java.util.List;

@GrpcService
public class TransferRecordServiceGrpcImpl
        extends TransferRecordServiceGrpc.TransferRecordServiceImplBase
{
    private final ITransferRecordService coreService;

    public TransferRecordServiceGrpcImpl(ITransferRecordService coreService)
    {
        this.coreService = coreService;
    }

    @Override
    public void addTransferRecord(TransferRecordCreationRequest request,
                                  StreamObserver<TransferRecordData> responseObserver)
    {
        try
        {
            TransferRecordCreationDTO creationDto =
                    GrpcMapper.convertTransferProtoCreationToDto(request);

            TransferRecordDataDTO createdDto =
                    coreService.addTransferRecord(creationDto);

            TransferRecordData response =
                    GrpcMapper.convertTransferDtoToProto(createdDto);

            responseObserver.onNext(response);
            responseObserver.onCompleted();
        }
        catch (Exception e)
        {
            responseObserver.onError(e);
        }
    }

    @Override
    public void getAllTransferRecords(Empty request,
                                      StreamObserver<TransferRecordList> responseObserver)
    {
        List<TransferRecordDataDTO> dtos = coreService.getAllTransferRecords();

        TransferRecordList.Builder listBuilder = TransferRecordList.newBuilder();

        for (TransferRecordDataDTO dto : dtos)
        {
            listBuilder.addRecords(GrpcMapper.convertTransferDtoToProto(dto));
        }

        responseObserver.onNext(listBuilder.build());
        responseObserver.onCompleted();
    }

    @Override
    public void getTransferRecordsForCow(TransferRecordsForCowRequest request,
                                         StreamObserver<TransferRecordList> responseObserver)
    {
        List<TransferRecordDataDTO> dtos =
                coreService.getTransferRecordsForCow(request.getCowId());

        TransferRecordList.Builder listBuilder = TransferRecordList.newBuilder();

        for (TransferRecordDataDTO dto : dtos)
        {
            listBuilder.addRecords(GrpcMapper.convertTransferDtoToProto(dto));
        }

        responseObserver.onNext(listBuilder.build());
        responseObserver.onCompleted();
    }

    @Override
    public void getTransferRecordById(TransferRecordIdRequest request,
                                      StreamObserver<TransferRecordData> responseObserver)
    {
        TransferRecordDataDTO dto =
                coreService.getTransferRecordById(request.getId());

        responseObserver.onNext(GrpcMapper.convertTransferDtoToProto(dto));
        responseObserver.onCompleted();
    }

    @Override
    public void approveTransfer(ApproveTransferRequest request,
                                StreamObserver<TransferRecordData> responseObserver)
    {
        TransferRecordDataDTO dto =
                coreService.approveTransfer(request.getTransferId(), request.getVetUserId());

        responseObserver.onNext(GrpcMapper.convertTransferDtoToProto(dto));
        responseObserver.onCompleted();
    }
}
