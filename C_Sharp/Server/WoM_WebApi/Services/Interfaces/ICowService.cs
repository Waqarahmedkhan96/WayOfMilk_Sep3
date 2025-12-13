// File: Server/WoM_WebApi/Services/Interfaces/ICowService.cs
using ApiContracts;

namespace WoM_WebApi.Services.Interfaces;

// Service: cow CRUD + queries
public interface ICowService
{
    Task<CowDto> CreateAsync(CreateCowDto dto, long requestedByUserId); // create cow
    Task<CowDto> GetByIdAsync(long id);                                 // get one
    Task<CowListDto> GetAllAsync();                                     // list all
    Task<CowDto> UpdateAsync(UpdateCowDto dto, long requestedByUserId); // update cow
    Task DeleteAsync(long id);                                          // delete cow
}
