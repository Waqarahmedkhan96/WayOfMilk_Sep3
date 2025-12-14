namespace WoM_BlazorApp.Services.Interfaces;

using ApiContracts;

public interface IDepartmentService
{
    Task<ICollection<DepartmentDto>> GetAllAsync();
}

