using ApiContracts;

namespace WoM_BlazorApp.Http;

public interface ICustomerService
{
    Task<ICollection<CustomerDto>> GetAllAsync();
    Task<CustomerDto> GetByIdAsync(int id);
    Task<CustomerDto> CreateAsync(CreateCustomerDto dto);
    Task UpdateAsync(int id, UpdateCustomerDto dto);
    Task DeleteAsync(int id);
}
