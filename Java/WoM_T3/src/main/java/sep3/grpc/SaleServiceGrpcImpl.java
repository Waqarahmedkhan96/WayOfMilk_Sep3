package sep3.grpc;

import io.grpc.stub.StreamObserver;
import net.devh.boot.grpc.server.service.GrpcService;
import sep3.mapping.GrpcMapper;
import sep3.service.interfaces.ISaleService;
import sep3.dto.saleDTO.SaleCreationDTO;
import sep3.dto.saleDTO.SaleDataDTO;
import sep3.wayofmilk.grpc.Empty;
import sep3.wayofmilk.grpc.SaleCreationRequest;
import sep3.wayofmilk.grpc.SaleData;
import sep3.wayofmilk.grpc.SaleIdRequest;
import sep3.wayofmilk.grpc.SaleList;
import sep3.wayofmilk.grpc.SaleServiceGrpc;

import java.util.List;

@GrpcService
public class SaleServiceGrpcImpl extends SaleServiceGrpc.SaleServiceImplBase {

    private final ISaleService coreService;

    public SaleServiceGrpcImpl(ISaleService coreService) {
        this.coreService = coreService;
    }

    // CREATE
    @Override
    public void createSale(SaleCreationRequest request,
                           StreamObserver<SaleData> responseObserver) {

        SaleCreationDTO creationDTO =
                GrpcMapper.convertSaleProtoCreationToDto(request);

        SaleDataDTO createdSale = coreService.addSale(creationDTO);

        SaleData response = GrpcMapper.convertSaleDtoToProto(createdSale);

        responseObserver.onNext(response);
        responseObserver.onCompleted();
    }

    // READ: Get by ID
    @Override
    public void getSaleById(SaleIdRequest request,
                            StreamObserver<SaleData> responseObserver) {

        long id = request.getId();

        SaleDataDTO dto = coreService.getSaleById(id);
        SaleData response = GrpcMapper.convertSaleDtoToProto(dto);

        responseObserver.onNext(response);
        responseObserver.onCompleted();
    }

    // READ: Get all
    @Override
    public void getAllSales(Empty request,
                            StreamObserver<SaleList> responseObserver) {

        List<SaleDataDTO> sales = coreService.getAllSales();

        SaleList.Builder listBuilder = SaleList.newBuilder();
        for (SaleDataDTO dto : sales) {
            listBuilder.addSales(GrpcMapper.convertSaleDtoToProto(dto));
        }

        responseObserver.onNext(listBuilder.build());
        responseObserver.onCompleted();
    }
}
