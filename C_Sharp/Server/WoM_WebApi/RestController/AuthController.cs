using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ApiContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    [HttpPost("login")]
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
    }

    // -----------------------------
    // PUBLIC REGISTRATION
    // Always Worker, no Vet/Owner
    // -----------------------------
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<UserDto>> Register(CreateUserDto dto)
    {
        // force Worker signup publicly
        dto.Role          = UserRole.Worker;  // enum, not string
        dto.LicenseNumber = null;

        var created = await _userService.CreateAsync(dto);
        return Ok(created);
    }

    // -----------------------------
    // CHANGE PASSWORD (SELF ONLY)
    // -----------------------------
    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
    {
        var sub = User.FindFirst(ClaimTypes.NameIdentifier)
                  ?? User.FindFirst(JwtRegisteredClaimNames.Sub);

        if (sub == null || !long.TryParse(sub.Value, out var userId))
            return Unauthorized();

        if (dto.UserId != userId)
            return Forbid();

        await _authService.ChangePasswordAsync(dto);
        return NoContent();
    }

    // -----------------------------
    // OWNER RESETS ANY USER PASSWORD
    // -----------------------------
    [HttpPost("reset-password")]
    [Authorize(Policy = "OwnerOnly")]
    public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
    {
        await _authService.ResetPasswordAsync(dto);
        return NoContent();
    }
}
