package sep3.Mapping;

import sep3.dto.customerDTO.CustomerDataDTO;
import sep3.entity.Customer;

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
}
