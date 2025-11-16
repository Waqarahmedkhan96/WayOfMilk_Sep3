using System.ComponentModel;
using ApiContracts.Containers;

namespace WoM_BlazorApp.Http;

public interface IContainerService
{
    Task<ICollection<ContainerDto>> GetAllAsync();
    Task<ContainerDto> GetByIdAsync(int id);
    Task<ContainerDto> CreateAsync(CreateContainerDto dto);
    Task UpdateAsync(int id, UpdateContainerDto dto);
    Task DeleteAsync(int id);
}
