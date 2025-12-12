using ApiContracts;

namespace WoM_BlazorApp.Http;

public interface IUserService
{
    Task<ICollection<UserDto>> GetAllAsync();
    Task<UserDto> GetByIdAsync(int id);
    Task<UserDto> CreateAsync(CreateUserDto dto);
    Task UpdateAsync(int id, UpdateUserDto dto);
    Task DeleteAsync(long id);
}
