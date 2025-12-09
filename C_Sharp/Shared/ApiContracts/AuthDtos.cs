using System;

namespace ApiContracts;

// DTO: login request (UI → WebApi)
public class LoginRequestDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}

// DTO: login response (WebApi → UI)
public class LoginResponseDto
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public UserRole Role { get; set; }
    public required string Token { get; set; } // JWT token string
}

// DTO: change own password
public class ChangePasswordDto
{
    public long UserId { get; set; }
    public required string OldPassword { get; set; }
    public required string NewPassword { get; set; }
}

// DTO: admin reset password
public class ResetPasswordDto
{
    public long UserId { get; set; }
    public required string NewPassword { get; set; }
}

// DTO: user from gRPC AuthService
public class AuthenticatedUserDto
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Address { get; set; }
    public required string Phone { get; set; }
    public UserRole Role { get; set; }
    public string? LicenseNumber { get; set; } // only for vets
}
