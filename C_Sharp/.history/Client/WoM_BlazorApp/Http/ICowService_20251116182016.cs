using ApiContracts.Cows;

namespace WoM_BlazorApp.Http;

public interface ICowService
{
    Task<ICollection<CowDto>> GetAllAsync();
    Task<CowDto> GetByIdAsync(int id);
    Task<CowDto> CreateAsync(CreateCowDto dto);
    Task UpdateAsync(int id, UpdateCowDto dto);
    Task DeleteAsync(int id);
}
