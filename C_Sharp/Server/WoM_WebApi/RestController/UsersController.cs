using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ApiContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WoM_WebApi.Services.Interfaces;

namespace WoM_WebApi.RestController;

[ApiController]
[Route("users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    // -----------------------------
    // GET current user
    // -----------------------------
    [HttpGet("current-user")]
    [Authorize]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        var sub = User.FindFirst(ClaimTypes.NameIdentifier)
                  ?? User.FindFirst(JwtRegisteredClaimNames.Sub);

        if (sub == null || !long.TryParse(sub.Value, out var id))
            return Unauthorized();

        var user = await _userService.GetByIdAsync(id);
        return Ok(user);
    }

    // -----------------------------
    // GET all users (OWNER ONLY)
    // -----------------------------
    [HttpGet]
    [Authorize(Policy = "OwnerOnly")]
    public async Task<ActionResult<UserListDto>> GetAll()
    {
        var list = await _userService.GetAllAsync();
        return Ok(list);
    }

    // -----------------------------
    // GET by id
    // Owner: any user
    // Worker/Vet: only self
    // -----------------------------
    [HttpGet("{id:long}")]
    [Authorize]
    public async Task<ActionResult<UserDto>> GetById(long id)
    {
        var sub = User.FindFirst(ClaimTypes.NameIdentifier)
                  ?? User.FindFirst(JwtRegisteredClaimNames.Sub);

        if (sub == null || !long.TryParse(sub.Value, out var currentId))
            return Unauthorized();

        var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;

        if (!string.Equals(roleClaim, "Owner", StringComparison.OrdinalIgnoreCase) &&
            id != currentId)
        {
            return Forbid();
        }

        var user = await _userService.GetByIdAsync(id);
        return Ok(user);
    }

    // -----------------------------
    // CREATE user (OWNER ONLY)
    // -----------------------------
    [HttpPost]
    [Authorize(Policy = "WorkerOrOwner")]
    public async Task<ActionResult<UserDto>> CreateUser(CreateUserDto dto)
    {
        // dto.Role is enum now
        if (dto.Role == UserRole.Owner)
            return BadRequest("Owner cannot be created via API.");

        var created = await _userService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // -----------------------------
    // UPDATE user (OWNER ONLY)
    // -----------------------------
    [HttpPut("{id:long}")]
    [Authorize(Policy = "WorkerOrOwner")]
    public async Task<ActionResult<UserDto>> UpdateUser(long id, UpdateUserDto dto)
    {
        dto.Id = id;

        // dto.Role is nullable enum
        if (dto.Role.HasValue && dto.Role.Value == UserRole.Owner)
            return BadRequest("Cannot change role to OWNER.");

        var updated = await _userService.UpdateAsync(dto);
        return Ok(updated);
    }

    // -----------------------------
    // DELETE user (OWNER ONLY)
    // -----------------------------
    [HttpDelete("{id:long}")]
    [Authorize(Policy = "OwnerOnly")]
    public async Task<IActionResult> DeleteUser(long id)
    {
        await _userService.DeleteAsync(id);
        return NoContent();
    }
}
