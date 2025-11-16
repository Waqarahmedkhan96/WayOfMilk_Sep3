using ApiContracts.Milk;
using Entities;

namespace WoM_BlazorApp.Http;

public interface IMilkService
{
    Task<ICollection<MilkDto>> GetAllAsync();
    Task<MilkDto> GetByIdAsync(int id);
    Task<MilkDto> CreateAsync(CreateMilkDto dto);
    Task UpdateAsync(int id, UpdateMilkDto dto);
    Task DeleteAsync(int id);
}
