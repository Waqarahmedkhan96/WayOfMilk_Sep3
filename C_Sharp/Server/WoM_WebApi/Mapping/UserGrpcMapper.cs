using ApiContracts;
using Sep3.WayOfMilk.Grpc;

namespace WoM_WebApi.Mapping;

// Mapper: User DTOs ↔ gRPC
public static class UserGrpcMapper
{
    // Map role string from Java → enum
    private static UserRole MapRoleStringToEnum(string role)
        => role.ToUpperInvariant() switch
        {
            "OWNER" => UserRole.Owner,
            "WORKER" => UserRole.Worker,
            "VET"    => UserRole.Vet,
            _        => UserRole.Worker
        };

    // Map enum → Java string
    private static string MapRoleEnumToString(UserRole role)
        => role switch
        {
            UserRole.Owner  => "OWNER",
            UserRole.Worker => "WORKER",
            UserRole.Vet    => "VET",
            _               => "WORKER"
        };

    // DTO → gRPC create
    public static UserCreationRequest ToGrpc(this CreateUserDto dto)
        => new UserCreationRequest
        {
            Name          = dto.Name,
            Email         = dto.Email,
            Phone         = dto.Phone,
            Address       = dto.Address,
            Password      = dto.Password,
            Role          = MapRoleEnumToString(dto.Role),        // enum → string
            LicenseNumber = dto.LicenseNumber ?? string.Empty
        };

    // gRPC → DTO (single user)
    public static UserDto ToDto(this UserData grpc)
        => new UserDto
        {
            Id            = grpc.Id,
            Name          = grpc.Name ?? string.Empty,
            Email         = grpc.Email ?? string.Empty,
            Phone         = grpc.Phone ?? string.Empty,
            Address       = grpc.Address ?? string.Empty,
            Role          = MapRoleStringToEnum(grpc.Role ?? "WORKER"), // string → enum
            LicenseNumber = grpc.LicenseNumber
        };

    // gRPC list → DTO list
    public static UserListDto ToListDto(this UserList grpc)
    {
        var result = new UserListDto();
        foreach (var u in grpc.Users)
        {
            result.Users.Add(u.ToDto());
        }
        return result;
    }

    // gRPC → Authenticated user (for login)
    public static AuthenticatedUserDto ToAuthenticatedUser(this UserData grpc)
        => new AuthenticatedUserDto
        {
            Id            = grpc.Id,
            Name          = grpc.Name ?? string.Empty,
            Email         = grpc.Email ?? string.Empty,
            Address       = grpc.Address ?? string.Empty,
            Phone         = grpc.Phone ?? string.Empty,
            Role          = MapRoleStringToEnum(grpc.Role ?? "WORKER"), // string → enum
            LicenseNumber = grpc.LicenseNumber
        };
}
