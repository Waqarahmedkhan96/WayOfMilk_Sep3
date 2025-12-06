package sep3.RequestHandlers.CustomerService;

import sep3.dto.customerDTO.CustomerCreationDTO;
import sep3.dto.customerDTO.CustomerDataDTO;

import java.util.List;

public interface ICustomerDataService
{
    // CREATE
    CustomerDataDTO addCustomer(CustomerCreationDTO customer);

    // READ
    CustomerDataDTO getCustomerById(long id);
    List<CustomerDataDTO> getAllCustomers();

    // UPDATE (optional, for future use)
    CustomerDataDTO updateCustomer(CustomerDataDTO customer);

    // DELETE (optional, future-safe)
    void deleteCustomer(long id);
}
