using ApiContracts;

namespace WoM_BlazorApp.Services.Interfaces;

public interface IContainerService
{
    Task<ContainerListDto> GetAllAsync();
    Task<ContainerDto> GetByIdAsync(long id);
    Task<ContainerDto> CreateAsync(CreateContainerDto dto);
    Task<ContainerDto> UpdateAsync(long id, UpdateContainerDto dto);
    Task DeleteAsync(long id);
}
