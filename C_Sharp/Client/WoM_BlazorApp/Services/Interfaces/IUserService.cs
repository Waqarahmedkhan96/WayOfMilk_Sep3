using ApiContracts;

namespace WoM_BlazorApp.Services.Interfaces;

public interface IUserService
{
    // READ
    Task<UserDto> GetCurrentUserAsync();
    Task<IEnumerable<UserDto>> GetAllAsync(); // Owner only
    Task<UserDto> GetByIdAsync(long id);

    // CREATE
    Task CreateAsync(CreateUserDto dto);

    // UPDATE
    Task UpdateAsync(long id, UpdateUserDto dto); // Owner updates others
    Task UpdateProfileAsync(UpdateUserDto dto);   // User updates self

    // DELETE
    Task DeleteAsync(long id);
}
