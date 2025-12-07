package sep3.Mapping;

import sep3.dto.saleDTO.SaleDataDTO;
import sep3.entity.Sale;

public class SaleMapper {

    private SaleMapper() {
    }

    // ========== Entity -> DTO (read/list) ==========
    public static SaleDataDTO convertSaleToDto(Sale sale) {
        if (sale == null) {
            return null;
        }

        Long customerId = (sale.getCustomer() != null) ? sale.getCustomer().getId() : null;
        Long containerId = (sale.getContainer() != null) ? sale.getContainer().getId() : null;
        Long createdByUserId = (sale.getCreatedBy() != null) ? sale.getCreatedBy().getId() : null;

        return new SaleDataDTO(
                sale.getId(),
                customerId,
                containerId,
                sale.getQuantityL(),
                sale.getPrice(),
                sale.getDateTime(),
                sale.isRecallCase(),
                createdByUserId
        );
    }

    // ========== DTO -> Entity (partial update) ==========
    public static void updateSaleFromDto(Sale sale, SaleDataDTO dto) {
        if (sale == null || dto == null) {
            return;
        }

        if (dto.getQuantityL() != null) {
            sale.setQuantityL(dto.getQuantityL());
        }

        if (dto.getPrice() != null) {
            sale.setPrice(dto.getPrice());
        }

        if (dto.getDateTime() != null) {
            sale.setDateTime(dto.getDateTime());
        }

        if (dto.getRecallCase() != null) {
            sale.setRecallCase(dto.getRecallCase());
        }

        // Normally we do NOT change relationships (customer/container/createdBy)
        // through an update DTO, so we leave those alone here on purpose.
    }
}
