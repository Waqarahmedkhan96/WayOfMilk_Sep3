// File: Server/WoM_WebApi/Services/Interfaces/IUserService.cs
using ApiContracts;

namespace WoM_WebApi.Services.Interfaces;

// Interface: user CRUD service
public interface IUserService
{
    Task<UserDto> CreateAsync(CreateUserDto dto); // create user
    Task<UserDto> GetByIdAsync(long id);          // get by id
    Task<IEnumerable<UserDto>> GetAllAsync();          // get all
    Task<UserDto> UpdateAsync(UpdateUserDto dto); // update user
    Task DeleteAsync(long id);                    // delete user
}
