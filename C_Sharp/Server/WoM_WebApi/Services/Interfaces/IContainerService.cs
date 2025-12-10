// File: Server/WoM_WebApi/Services/Interfaces/IContainerService.cs
using ApiContracts;

namespace WoM_WebApi.Services.Interfaces;

// Service: containers CRUD
public interface IContainerService
{
    Task<ContainerDto> CreateAsync(CreateContainerDto dto); // create container
    Task<ContainerDto> GetByIdAsync(long id);               // get one
    Task<ContainerListDto> GetAllAsync();                   // list all
    Task<ContainerDto> UpdateAsync(long id, UpdateContainerDto dto); // update
    Task DeleteAsync(long id);                              // delete
}
