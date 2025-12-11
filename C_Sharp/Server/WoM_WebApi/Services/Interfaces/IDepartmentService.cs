using ApiContracts;

namespace WoM_WebApi.Services.Interfaces;

// Service: departments
public interface IDepartmentService
{
    Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto);     // create dept
    Task<DepartmentDto> GetByIdAsync(long id);                    // get one
    Task<DepartmentListDto> GetAllAsync();                        // list all
    Task<DepartmentListDto> GetByTypeAsync(DepartmentType type);  // filter by type
    Task<DepartmentDto> UpdateAsync(long id, UpdateDepartmentDto dto); // update
    Task DeleteAsync(long id);                                    // delete
}
