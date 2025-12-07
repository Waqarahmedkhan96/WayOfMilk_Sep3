package sep3.service.impl;

import org.springframework.stereotype.Service;
import sep3.mapping.CustomerMapper;
import sep3.repository.UserRepository;
import sep3.dto.customerDTO.CustomerCreationDTO;
import sep3.dto.customerDTO.CustomerDataDTO;
import sep3.entity.Customer;
import sep3.entity.user.User;
import sep3.service.interfaces.ICustomerService;

import java.util.List;
import java.util.stream.Collectors;

@Service
public class CustomerServiceImpl implements ICustomerService {

    private final sep3.dao.CustomerRepository customerRepository;
    private final UserRepository userRepository;

    public CustomerServiceImpl(sep3.dao.CustomerRepository customerRepository,
                               UserRepository userRepository) {
        this.customerRepository = customerRepository;
        this.userRepository = userRepository;
    }

    // ========== CREATE ==========
    @Override
    public CustomerDataDTO addCustomer(CustomerCreationDTO dto)
    {
        if (dto.getCompanyName() == null || dto.getPhoneNo() == null ||
                dto.getEmail() == null || dto.getCompanyCVR() == null ||
                dto.getRegisteredByUserId() == null)
        {
            throw new IllegalArgumentException("All customer fields must be provided.");
        }

        User registeredBy = userRepository.findById(dto.getRegisteredByUserId())
                .orElseThrow(() ->
                        new RuntimeException("User not found: " + dto.getRegisteredByUserId()));

        Customer customer = new Customer(
                dto.getCompanyName(),
                dto.getPhoneNo(),
                dto.getEmail(),
                dto.getCompanyCVR(),
                registeredBy
        );

        Customer saved = customerRepository.save(customer);

        return CustomerMapper.convertCustomerToDto(saved);
    }

    // ========== READ ==========
    @Override
    public CustomerDataDTO getCustomerById(long id)
    {
        Customer customer = customerRepository.findById(id)
                .orElseThrow(() -> new RuntimeException("Customer not found: " + id));

        return CustomerMapper.convertCustomerToDto(customer);
    }

    @Override
    public List<CustomerDataDTO> getAllCustomers()
    {
        return customerRepository.findAll()
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

        Customer customer = customerRepository.findById(dto.getId())
                .orElseThrow(() -> new RuntimeException("Customer not found: " + dto.getId()));

        CustomerMapper.updateCustomerFromDto(customer, dto);

        Customer updated = customerRepository.save(customer);
        return CustomerMapper.convertCustomerToDto(updated);
    }

    // ========== DELETE ==========
    @Override
    public void deleteCustomer(long id)
    {
        if (!customerRepository.existsById(id))
        {
            throw new RuntimeException("Customer not found: " + id);
        }
        customerRepository.deleteById(id);
    }
}
