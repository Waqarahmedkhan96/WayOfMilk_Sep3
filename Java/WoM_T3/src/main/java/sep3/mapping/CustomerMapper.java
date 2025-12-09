package sep3.mapping;

import sep3.dto.customerDTO.CustomerDataDTO;
import sep3.entity.Customer;

import java.util.List;
import java.util.stream.Collectors;

public class CustomerMapper {

    private CustomerMapper() {
    }

    // ========== Entity -> DTO (read/list) ==========
    public static CustomerDataDTO convertCustomerToDto(Customer customer) {
        if (customer == null) {
            return null;
        }

        return new CustomerDataDTO(
                customer.getId(),
                customer.getCompanyName(),
                customer.getPhoneNo(),
                customer.getEmail(),
                customer.getCompanyCVR()
        );
    }

    // ========== DTO -> Entity (partial update) ==========
    public static void updateCustomerFromDto(Customer customer, CustomerDataDTO dto) {
        if (customer == null || dto == null) {
            return;
        }

        if (dto.getCompanyName() != null && !dto.getCompanyName().isBlank()) {
            customer.setCompanyName(dto.getCompanyName());
        }

        if (dto.getPhoneNo() != null && !dto.getPhoneNo().isBlank()) {
            customer.setPhoneNo(dto.getPhoneNo());
        }

        if (dto.getEmail() != null && !dto.getEmail().isBlank()) {
            customer.setEmail(dto.getEmail());
        }

        if (dto.getCompanyCVR() != null && !dto.getCompanyCVR().isBlank()) {
            customer.setCompanyCVR(dto.getCompanyCVR());
        }
    }

    public static List<CustomerDataDTO> convertCustomerListToDto(List<Customer> customerList) {
        return customerList.stream().map(CustomerMapper::convertCustomerToDto).toList();
    }

    /*
    public static List<Customer> convertDtoListToEntity(List<CustomerDataDTO> customerList) {
        return customerList.stream().map(CustomerMapper::convertCustomerToDto).collect(Collectors.toList());
    }
     */
}
