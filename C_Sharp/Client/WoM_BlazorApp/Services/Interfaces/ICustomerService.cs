using ApiContracts;

namespace WoM_BlazorApp.Services.Interfaces;

public interface ICustomerService
{
    Task<ICollection<CustomerDto>> GetAllAsync();
    Task<CustomerDto> GetByIdAsync(int id);
    Task<CustomerDto> CreateAsync(CreateCustomerDto dto);
    Task UpdateAsync(int id, UpdateCustomerDto dto);
    Task DeleteAsync(long id);
}
