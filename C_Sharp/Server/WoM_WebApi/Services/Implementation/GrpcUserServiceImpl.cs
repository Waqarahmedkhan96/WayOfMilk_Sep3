// File: Server/WoM_WebApi/Services/Implementation/GrpcUserServiceImpl.cs
using ApiContracts;
using Sep3.WayOfMilk.Grpc;
using WoM_WebApi.GlobalExceptionHandler;
using WoM_WebApi.Mapping;
using WoM_WebApi.Services.Interfaces;

namespace WoM_WebApi.Services.Implementation;

// Impl: gRPC-based user CRUD
public class GrpcUserServiceImpl : IUserService
{
    private readonly UserService.UserServiceClient _client;

    public GrpcUserServiceImpl(UserService.UserServiceClient client)
    {
        _client = client;
    }

    // -----------------------------
    // CREATE new user
    // -----------------------------
    public async Task<UserDto> CreateAsync(CreateUserDto dto)
    {
        // DTO → gRPC
        var request = dto.ToGrpc();

        var reply = await _client.AddUserAsync(request);

        if (reply == null || reply.Id == 0)
            throw new ValidationException("Unable to create user.");

        return reply.ToDto();
    }

    // -----------------------------
    // GET by id
    // -----------------------------
    public async Task<UserDto> GetByIdAsync(long id)
    {
        var reply = await _client.GetUserByIdAsync(new SentId { Id = id });

        if (reply == null || reply.Id == 0)
            throw new NotFoundException($"User with id {id} not found.");

        return reply.ToDto();
    }

    // -----------------------------
    // GET all users
    // -----------------------------
    public async Task<UserListDto> GetAllAsync()
    {
        var reply = await _client.GetAllUsersAsync(new Empty());
        return reply.ToListDto();
    }

    // -----------------------------
    // UPDATE existing user
    // -----------------------------
    public async Task<UserDto> UpdateAsync(UpdateUserDto dto)
    {
        // Get current from gRPC
        var current = await _client.GetUserByIdAsync(new SentId { Id = dto.Id });

        if (current == null || current.Id == 0)
            throw new NotFoundException($"User with id {dto.Id} not found.");

        // Apply changes
        if (dto.Name is not null)    current.Name = dto.Name;
        if (dto.Email is not null)   current.Email = dto.Email;
        if (dto.Phone is not null)   current.Phone = dto.Phone;
        if (dto.Address is not null) current.Address = dto.Address;

        if (dto.Role.HasValue)
        {
            // enum → string for gRPC
            current.Role = dto.Role.Value switch
            {
                UserRole.Owner  => "OWNER",
                UserRole.Worker => "WORKER",
                UserRole.Vet    => "VET",
                _               => "WORKER"
            };
        }

        if (dto.LicenseNumber is not null)
            current.LicenseNumber = dto.LicenseNumber;

        var reply = await _client.UpdateUserAsync(current);
        return reply.ToDto();
    }

    // -----------------------------
    // DELETE user
    // -----------------------------
    public async Task DeleteAsync(long id)
    {
        var existing = await _client.GetUserByIdAsync(new SentId { Id = id });

        if (existing == null || existing.Id == 0)
            throw new NotFoundException($"User with id {id} not found.");

        await _client.DeleteUserAsync(new SentId { Id = id });
    }
}
