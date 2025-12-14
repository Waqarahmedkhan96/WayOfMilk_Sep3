using ApiContracts;

namespace WoM_BlazorApp.Services.Interfaces;

public interface ISaleService
{
    Task<ICollection<SaleDto>> GetAllAsync();
    Task<SaleDto> GetByIdAsync(long id);
    Task<SaleDto> CreateAsync(CreateSaleDto dto);
    Task UpdateAsync(long id, UpdateSaleDto dto);
    Task DeleteAsync(long id);

    //for mocking
    Task<IEnumerable<SaleDto>> GetAllTrackedAsync();
    Task<SaleDto> GetTrackedByIdAsync(long id);
}
