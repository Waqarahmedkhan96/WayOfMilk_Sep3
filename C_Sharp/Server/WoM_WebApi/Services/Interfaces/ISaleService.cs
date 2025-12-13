using ApiContracts;

namespace WoM_WebApi.Services.Interfaces;

// Service: sales
public interface ISaleService
{
    Task<SaleDto> CreateAsync(CreateSaleDto dto);                 // create sale
    Task<SaleDto> GetByIdAsync(long id);                          // get one
    Task<SaleListDto> GetAllAsync();                              // list all
    Task DeleteAsync(long id);                                    // delete sale
}