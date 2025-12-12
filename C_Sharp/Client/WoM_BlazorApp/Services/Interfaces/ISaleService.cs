using ApiContracts;

namespace WoM_BlazorApp.Http;

public interface ISaleService
{
    Task<ICollection<SaleDto>> GetAllAsync();
    Task<SaleDto> GetByIdAsync(int id);
    Task<SaleDto> CreateAsync(CreateSaleDto dto);
    Task UpdateAsync(int id, UpdateSaleDto dto);
    Task DeleteAsync(long id);
}
