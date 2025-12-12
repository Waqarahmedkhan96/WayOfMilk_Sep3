using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ApiContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WoM_WebApi.Helper;
using WoM_WebApi.Log;
using WoM_WebApi.Services.Interfaces;

namespace WoM_WebApi.RestController;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ITokenService _tokenService;
    private readonly IUserService _userService;

    public AuthController(
        IAuthService authService,
        ITokenService tokenService,
        IUserService userService)
    {
        _authService  = authService;
        _tokenService = tokenService;
        _userService  = userService;
    }

    // -----------------------------
    // PUBLIC LOGIN
    // -----------------------------
    [HttpPost("login", Name = "Login")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponseDto>> Login(LoginRequestDto request)
    {
        var user  = await _authService.AuthenticateAsync(request); // gRPC → DTO
        var token = _tokenService.GenerateToken(user);             // JWT

        return Ok(new LoginResponseDto
        {
            Id    = user.Id,
            Name  = user.Name,
            Email = user.Email,
            Role  = user.Role, // UserRole → UserRole (OK)
            Token = token
        });
        //no loggers here since the grpc implementation is already logging
    }

    // -----------------------------
    // PUBLIC REGISTRATION
    // Always Worker, no Vet/Owner
    // -----------------------------
    [HttpPost("register", Name = "Register")]
    [AllowAnonymous]
    public async Task<ActionResult<UserDto>> Register(CreateUserDto dto)
    {
        ActivityLog.Instance.Log("registering new user", $"Attempt to register new user with email {dto.Email} and name {dto.Name}", dto.Name, 0);
        // force Worker signup publicly
        dto.Role          = UserRole.Worker;  // enum, not string
        dto.LicenseNumber = null;

        var created = await _userService.CreateAsync(dto);

        return Ok(created);
    }

    // -----------------------------
    // CHANGE PASSWORD (SELF ONLY)
    // -----------------------------
    [HttpPost("change-password", Name = "ChangePassword")]
    [Authorize]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
    {
        var sub = User.FindFirst(ClaimTypes.NameIdentifier)
                  ?? User.FindFirst(JwtRegisteredClaimNames.Sub);
        // Try to get the name for better logs (optional)
        var nameClaim = User.FindFirst(ClaimTypes.Name)?.Value ?? "Unknown";

        if (sub == null || !long.TryParse(sub.Value, out var userId))
        {
            ActivityLog.Instance.Log(
                            "Auth Error",
                            "Token contained invalid ID during password change",
                            nameClaim,
                            0
                        );
                        return Unauthorized();
        }

        if (dto.UserId != userId)
        {
            // IMPORTANT: Log this! This is a user trying to hack/modify someone else's account.
            ActivityLog.Instance.Log(
                "Access Denied",
                $"User {userId} tried to change password for target ID {dto.UserId}",
                nameClaim,
                userId
            );
            return Forbid();
        }

        // Force the operation to apply to the logged-in user
        //the id is taken from the token, since the user is already logged in at this step
        // safer
        dto.UserId = userId;

        await _authService.ChangePasswordAsync(dto);
        return NoContent();
    }

    // -----------------------------
    // OWNER RESETS ANY USER PASSWORD
    // -----------------------------
    [HttpPost("reset-password", Name = "ResetPassword")]
    [Authorize(Policy = "OwnerOnly")]
    public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
    {
        await _authService.ResetPasswordAsync(dto);
        ActivityLog.Instance.Log("Reset Password", $"Password reset by owner {User.GetJWTNameAndId()} for user ID {dto.UserId}", User.GetJWTName(), User.GetJWTId());
        return NoContent();
    }
}
