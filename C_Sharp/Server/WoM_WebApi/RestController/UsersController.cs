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

    // --- HELPER METHOD ---
    // Extracts the user ID safely from the JWT token
    //separated to avoid boilerplate code in other methods
    //since it repeats a lot of times
    private long? GetCurrentUserId()
    {
        var sub = User.FindFirst(ClaimTypes.NameIdentifier)
                  ?? User.FindFirst(JwtRegisteredClaimNames.Sub);

        if (sub != null && long.TryParse(sub.Value, out var id))
        {
            return id;
        }
        return null;
    }

    // -----------------------------
    // GET current user
    // -----------------------------
    [HttpGet("current-user")]
    [Authorize]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        var id = GetCurrentUserId();
        if (id == null) return Unauthorized();

        var user = await _userService.GetByIdAsync(id.Value);
        return Ok(user);
    }


    // -----------------------------
    // GET all users (OWNER ONLY)
    // -----------------------------
    [HttpGet]
    [Authorize(Policy = "OwnerOnly")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
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
        var currentId = GetCurrentUserId();
        if (currentId == null) return Unauthorized();

        var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
        bool isOwner = string.Equals(roleClaim, "Owner", StringComparison.OrdinalIgnoreCase);

        if (!isOwner && id != currentId)
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
        //owner can create ANY type of user
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
        //if (dto.Role.HasValue && dto.Role.Value == UserRole.Owner)
        //return BadRequest("Cannot change role to OWNER.");

        var updated = await _userService.UpdateAsync(dto);
        return Ok(updated);
    }

    // Any logged-in user can change their own Name, Email, Phone, Address
// -----------------------------
    [HttpPut("profile")] // Route: PUT /users/profile
    [Authorize]
    public async Task<ActionResult<UserDto>> UpdateProfile(UpdateUserDto dto)
    {
        // Get ID from Token (Security: Trust the token, not the DTO)
        var currentId = GetCurrentUserId();
        if (currentId == null) return Unauthorized();

        // Force the ID to match the token
        dto.Id = currentId.Value;

        // SECURITY: Prevent Role Escalation
        // We explicitly nullify the Role and LicenseNumber so the Service ignores them.
        dto.Role = null;
        dto.LicenseNumber = null;

        // The service will update the basic fields but skip Role/License
        // because they are null.
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
