namespace WoM_BlazorApp.Services.Interfaces;

using ApiContracts;

public interface IDepartmentService
{
    Task<ICollection<DepartmentDto>> GetAllAsync();
    Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto);
    Task<DepartmentDto> UpdateAsync(long id, UpdateDepartmentDto dto);
    Task DeleteAsync(long id);

}

