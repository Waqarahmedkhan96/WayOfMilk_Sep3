using ApiContracts;


namespace WoM_BlazorApp.Http;

public interface ICowService
{
    Task<ICollection<CowDto>> GetAllAsync();
    Task<CowDto> GetByIdAsync(int id);
    Task<CowDto> CreateAsync(CowCreationDto dto);
    Task UpdateAsync(int id, UpdateCowDto dto);
    Task DeleteAsync(long id);
}
