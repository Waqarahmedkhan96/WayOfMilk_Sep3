using ApiContracts;

namespace WoM_BlazorApp.Services.Interfaces;

public interface ICowService
{
    Task<IEnumerable<CowDto>> GetAllAsync();
    Task<CowDto> GetByIdAsync(int id);
    Task<CowDto> CreateAsync(CowCreationDto dto);
    Task UpdateAsync(int id, CowDto dto);
    Task DeleteAsync(long id);
}
