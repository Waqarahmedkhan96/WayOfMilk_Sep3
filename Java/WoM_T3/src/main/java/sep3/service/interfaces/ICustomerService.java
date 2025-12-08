package sep3.service.interfaces;

import sep3.dto.customerDTO.CustomerCreationDTO;
import sep3.dto.customerDTO.CustomerDataDTO;

import java.util.List;

public interface ICustomerService
{
    // CREATE
    CustomerDataDTO addCustomer(CustomerCreationDTO customer);

    // READ
    CustomerDataDTO getCustomerById(long id);
    List<CustomerDataDTO> getAllCustomers();
    CustomerDataDTO getCustomerByCVR(String cvr);
    List<CustomerDataDTO> getCustomersByName(String name);

    // UPDATE (optional, for future use)
    CustomerDataDTO updateCustomer(CustomerDataDTO customer);

    // DELETE (optional, future-safe)
    void deleteCustomer(long id);
}
