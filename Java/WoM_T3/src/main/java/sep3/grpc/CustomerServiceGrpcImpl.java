package sep3.grpc;

import io.grpc.stub.StreamObserver;
import net.devh.boot.grpc.server.service.GrpcService;
import sep3.mapping.GrpcMapper;
import sep3.service.interfaces.ICustomerService;
import sep3.dto.customerDTO.CustomerCreationDTO;
import sep3.dto.customerDTO.CustomerDataDTO;
import sep3.wayofmilk.grpc.*;

import java.util.List;

@GrpcService
public class CustomerServiceGrpcImpl extends CustomerServiceGrpc.CustomerServiceImplBase {

    private final ICustomerService coreService;

    public CustomerServiceGrpcImpl(ICustomerService coreService) {
        this.coreService = coreService;
    }

    // CREATE
    @Override
    public void createCustomer(CustomerCreationRequest request,
                               StreamObserver<CustomerData> responseObserver) {


        // Proto -> DTO
        CustomerCreationDTO creationDTO =
                GrpcMapper.convertCustomerProtoCreationToDto(request);

        // Business logic
        CustomerDataDTO createdCustomer = coreService.addCustomer(creationDTO);

        // DTO -> Proto
        CustomerData response = GrpcMapper.convertCustomerDtoToProto(createdCustomer);

        responseObserver.onNext(response);
        responseObserver.onCompleted();
    }

    // READ: Get by ID
    @Override
    public void getCustomerById(CustomerIdRequest request,
                                StreamObserver<CustomerData> responseObserver) {

        long id = request.getId();

        CustomerDataDTO dto = coreService.getCustomerById(id);
        CustomerData response = GrpcMapper.convertCustomerDtoToProto(dto);

        responseObserver.onNext(response);
        responseObserver.onCompleted();
    }

    //READ: get by CVR
    @Override
    public void getCustomerByCVR (SentString cvr, StreamObserver<CustomerData> responseObserver)
    {
        if (cvr.getValue().isEmpty()) {
            throw new IllegalArgumentException("Please add a CVR value to search for.");
        }
        String cvrString = cvr.getValue();
        CustomerDataDTO dto = coreService.getCustomerByCVR(cvrString);
        CustomerData response = GrpcMapper.convertCustomerDtoToProto(dto);

        responseObserver.onNext(response);
        responseObserver.onCompleted();
    }


    // READ: Get all
    @Override
    public void getAllCustomers(Empty request,
                                StreamObserver<CustomerList> responseObserver) {

        List<CustomerDataDTO> customers = coreService.getAllCustomers();

        CustomerList.Builder listBuilder = CustomerList.newBuilder();
        for (CustomerDataDTO dto : customers) {
            listBuilder.addCustomers(GrpcMapper.convertCustomerDtoToProto(dto));
        }

        responseObserver.onNext(listBuilder.build());
        responseObserver.onCompleted();
    }

    //READ: get by name
    @Override
    public void getCustomerByName(SentString name, StreamObserver<CustomerList> responseObserver)
    {
        List<CustomerDataDTO> foundCustomers = coreService.getCustomersByName(name.getValue());
        CustomerList.Builder listBuilder = CustomerList.newBuilder();
        for (CustomerDataDTO dto : foundCustomers) {
            listBuilder.addCustomers(GrpcMapper.convertCustomerDtoToProto(dto));
        }

        responseObserver.onNext(listBuilder.build());
        responseObserver.onCompleted();
    }

    //DELETE
    @Override
    public void deleteCustomer(SentId id, StreamObserver<Empty> streamObserver)
    {
        coreService.deleteCustomer(id.getId());

        streamObserver.onNext(Empty.getDefaultInstance());
        streamObserver.onCompleted();
    }
}
