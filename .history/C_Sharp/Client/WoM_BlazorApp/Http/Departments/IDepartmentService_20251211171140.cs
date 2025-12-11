using ApiContracts.Departments;

namespace WoM_BlazorApp.Http;

public interface IDepartmentService
{
    Task<ICollection<DepartmentDto>> GetAllAsync();
    Task<DepartmentDto> GetByIdAsync(long id);
    Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto);
    Task<DepartmentDto> UpdateAsync(long id, UpdateDepartmentDto dto);
    Task DeleteAsync(long id);
    Task<ICollection<DepartmentDto>> GetByTypeAsync(DepartmentType type);
    Task<ICollection<CowDto>> GetCowsByDepartmentAsync(long departmentId);
    Task<ICollection<TransferRecordDto>> GetTransferRecordsByDepartmentAsync(long departmentId);
}
