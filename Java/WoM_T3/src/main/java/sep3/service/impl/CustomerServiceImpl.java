package sep3.service.impl;

import io.grpc.stub.StreamObserver;
import net.devh.boot.grpc.server.service.GrpcService;
import org.springframework.stereotype.Service;
import sep3.mapping.CustomerMapper;
import sep3.mapping.GrpcMapper;
import sep3.repository.CustomerRepository;
import sep3.repository.UserRepository;
import sep3.dto.customerDTO.CustomerCreationDTO;
import sep3.dto.customerDTO.CustomerDataDTO;
import sep3.entity.Customer;
import sep3.entity.user.User;
import sep3.service.interfaces.ICustomerService;
import sep3.wayofmilk.grpc.*;

import java.util.List;
import java.util.stream.Collectors;

@Service
public class CustomerServiceImpl implements ICustomerService {

    private final CustomerRepository customerDAO;

    public CustomerServiceImpl(CustomerRepository customerDAO) {
        this.customerDAO = customerDAO;
    }

    // ========== CREATE ==========
    @Override
    public CustomerDataDTO addCustomer(CustomerCreationDTO dto)
    {
        // Validate
        if (dto.getCompanyName() == null || dto.getPhoneNo() == null ||
                dto.getEmail() == null || dto.getCompanyCVR() == null)
        {
            throw new IllegalArgumentException("All customer fields must be provided.");
        }


        // Create Customer using your PUBLIC constructor
        Customer customer = new Customer(
                dto.getCompanyName(),
                dto.getPhoneNo(),
                dto.getEmail(),
                dto.getCompanyCVR(),
                registeredBy
        );

        Customer saved = customerDAO.save(customer);

        //use existing mapper method name
        return CustomerMapper.convertCustomerToDto(saved);
    }

    // ========== READ ==========
    @Override
    public CustomerDataDTO getCustomerById(long id)
    {
        Customer customer = customerDAO.findById(id)
                .orElseThrow(() -> new RuntimeException("Customer not found: " + id));

        return CustomerMapper.convertCustomerToDto(customer);
    }

    @Override
    public List<CustomerDataDTO> getAllCustomers()
    {
        return customerDAO.findAll()
                .stream()
                .map(CustomerMapper::convertCustomerToDto)
                .collect(Collectors.toList());
    }

    // ========== UPDATE (partial) ==========
    @Override
    public CustomerDataDTO updateCustomer(CustomerDataDTO dto)
    {
        if (dto.getId() == null)
        {
            throw new IllegalArgumentException("Customer ID must be provided for update.");
        }

        Customer customer = customerDAO.findById(dto.getId())
                .orElseThrow(() -> new RuntimeException("Customer not found: " + dto.getId()));

        CustomerMapper.updateCustomerFromDto(customer, dto);

        Customer updated = customerDAO.save(customer);
        return CustomerMapper.convertCustomerToDto(updated);
    }

    // ========== DELETE ==========
    @Override
    public void deleteCustomer(long id)
    {
        if (!customerDAO.existsById(id))
        {
            throw new RuntimeException("Customer not found: " + id);
        }
        customerDAO.deleteById(id);
    }
}