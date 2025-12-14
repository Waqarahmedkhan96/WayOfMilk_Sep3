using ApiContracts;

namespace WoM_BlazorApp.Services.Interfaces;

public interface ICowService
{
    // Basic CRUD
    Task<IEnumerable<CowDto>> GetAllAsync();
    Task<CowDto> GetByIdAsync(long id);
    Task CreateAsync(CowCreationDto dto);
    Task UpdateAsync(long id, CowDto dto);
    Task DeleteAsync(long id);

    // Special Actions
    Task UpdateHealthAsync(IEnumerable<long> cowIds, bool newStatus);

    // Other Read Methods
    Task<IEnumerable<CowDto>> GetCowsByDepartmentAsync(long departmentId);
    Task<IEnumerable<MilkDto>> GetMilkByCowIdAsync(long cowId);
}
