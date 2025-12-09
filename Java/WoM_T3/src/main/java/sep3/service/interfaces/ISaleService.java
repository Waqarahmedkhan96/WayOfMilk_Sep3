package sep3.service.interfaces;

import sep3.dto.saleDTO.SaleCreationDTO;
import sep3.dto.saleDTO.SaleDataDTO;

import java.util.List;

public interface ISaleService
{
    // CREATE
    SaleDataDTO addSale(SaleCreationDTO sale);

    // READ
    SaleDataDTO getSaleById(long id);
    List<SaleDataDTO> getAllSales();

    // UPDATE (optional)
    SaleDataDTO updateSale(SaleDataDTO sale);

    // DELETE (optional)
    void deleteSale(long id);
}
