namespace ApiContracts;

// Login request from UI to WebApi
public class LoginRequestDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}

// Login response from WebApi to UI

public class LoginResponseDto
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public UserRole Role { get; set; }
    public required string Token { get; set; } // NEW: JWT token
}

// User changes own password
public class ChangePasswordDto
{
    public long UserId { get; set; }
    public required string OldPassword { get; set; }
    public required string NewPassword { get; set; }
}

// Admin resets employee password
public class ResetPasswordDto
{
    public long UserId { get; set; }
    public required string NewPassword { get; set; }
}

//UserData from t3 grpc
public class AuthenticatedUserDto
{
    public long UserId { get; set; }
    public string Name { get; set; }
    public string email { get; set; }
    public string address { get; set }
    publicString phone { get; set; }
    public UserRole Role { get; set; }
    public string licenseNumber { get; set; }
}
