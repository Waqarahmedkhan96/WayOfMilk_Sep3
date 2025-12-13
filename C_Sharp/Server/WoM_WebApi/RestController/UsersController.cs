using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ApiContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WoM_WebApi.Log;
using WoM_WebApi.Services.Interfaces;
using WoM_WebApi.Helper;

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

    /* replaced with a static extension method in UserService.cs
    // --- HELPER METHODS ---
    // Extracts the user ID safely from the JWT token
    //separated to avoid boilerplate code in other methods
    //since it repeats a lot of times
    private long GetCurrentUserId()
    {
        var sub = User.FindFirst(ClaimTypes.NameIdentifier)
                  ?? User.FindFirst(JwtRegisteredClaimNames.Sub);

        if (sub != null && long.TryParse(sub.Value, out var id))
        {
            return id;
        }
        return 0;
        //if nothing found, return 0
    }

    //extracts the user name and id from the JWT token, for logging purposes
    private string GetCurrentUserNameAndId()
    {
        var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)
                      ?? User.FindFirst(JwtRegisteredClaimNames.Sub);

        var nameClaim = User.FindFirst(ClaimTypes.Name)
                        ?? User.FindFirst(JwtRegisteredClaimNames.Name)
                        ?? User.FindFirst("name"); // Fallback for some JWT providers

        if (idClaim != null)
        {
            var name = nameClaim?.Value ?? "Unknown";
            return $"{name} id {idClaim.Value}";
        }

        return "Anonymous";
    }
    */

    // -----------------------------
    // GET current user
    // -----------------------------
    [HttpGet("current-user", Name = "GetCurrentUser")]
    [Authorize]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        var id = User.GetJWTId();
        //using static extension method
        if (id == 0) return Unauthorized();

        var user = await _userService.GetByIdAsync(id);
        return Ok(user);
    }


    // -----------------------------
    // GET all users (OWNER ONLY)
    // -----------------------------
    [HttpGet(Name = "GetAllUsers")]
    [Authorize(Roles = "Owner")]
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
    [HttpGet("{id:long}", Name = "GetUserById")]
    [Authorize]
    public async Task<ActionResult<UserDto>> GetById(long id)
    {
        var currentId = User.GetJWTId();
        if (currentId == 0) return Unauthorized();

        var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
        bool isOwner = string.Equals(roleClaim, "Owner", StringComparison.OrdinalIgnoreCase);

        if (!isOwner && id != currentId)
        {
            ActivityLog.Instance.Log("Unauthorized", $"User {currentId} tried to access user {id}", User.GetJWTName(), currentId);
            return Forbid();
        }

        var user = await _userService.GetByIdAsync(id);
        return Ok(user);
    }

    // -----------------------------
    // CREATE user (OWNER ONLY)
    // -----------------------------
    [HttpPost(Name = "CreateUser")]
    [Authorize(Roles = "Owner")]
    public async Task<ActionResult<UserDto>> CreateUser(CreateUserDto dto)
    {
        // dto.Role is enum now
        //owner can create ANY type of user
        ActivityLog.Instance.Log("creating new user", $"{User.GetJWTNameAndId()} is attempting to create new {dto.Role} user with email {dto.Email} and name {dto.Name}", User.GetJWTName(), User.GetJWTId());
        var created = await _userService.CreateAsync(dto);
        ActivityLog.Instance.Log("created new user", $"{User.GetJWTNameAndId()} created new {dto.Role} user with email {dto.Email} and name {dto.Name}", User.GetJWTName(), User.GetJWTId());
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // -----------------------------
    // UPDATE user (OWNER ONLY)
    // -----------------------------
    [HttpPut("{id:long}", Name = "UpdateUser")]
    [Authorize(Roles = "Owner")]
    public async Task<ActionResult<UserDto>> UpdateUser(long id, UpdateUserDto dto)
    {
        ActivityLog.Instance.Log("updating user", $"{User.GetJWTNameAndId()} is attempting to update user {id}", User.GetJWTName(), id);
        dto.Id = id;

        // dto.Role is nullable enum
        //if (dto.Role.HasValue && dto.Role.Value == UserRole.Owner)
        //return BadRequest("Cannot change role to OWNER.");

        var updated = await _userService.UpdateAsync(dto);
        ActivityLog.Instance.Log("updated user", $"{User.GetJWTNameAndId()} updated user {id}", dto.Name, updated.Id);
        return Ok(updated);
    }

    // Any logged-in user can change their own Name, Email, Phone, Address
// -----------------------------
    [HttpPut("profile", Name = "UpdateUserProfile")] // Route: PUT /users/profile
    [Authorize]
    public async Task<ActionResult<UserDto>> UpdateProfile(UpdateUserDto dto)
    {
        ActivityLog.Instance.Log("updating personal profile", $"{User.GetJWTNameAndId()} is attempting to update profile", User.GetJWTName(), User.GetJWTId());
        // Get ID from Token (Security: Trust the token, not the DTO)
        var currentId = User.GetJWTId();
        if (currentId == 0) return Unauthorized();

        // Force the ID to match the token
        dto.Id = currentId;

        // SECURITY: Prevent Role Escalation
        // We explicitly nullify the Role and LicenseNumber so the Service ignores them.
        dto.Role = null;
        dto.LicenseNumber = null;

        // The service will update the basic fields but skip Role/License
        // because they are null.
        var updated = await _userService.UpdateAsync(dto);
        ActivityLog.Instance.Log("updated personal profile", $"{User.GetJWTNameAndId()} updated profile", User.GetJWTName(), User.GetJWTId());

        return Ok(updated);
    }

    // -----------------------------
    // DELETE user (OWNER ONLY)
    // -----------------------------
    [HttpDelete("{id:long}", Name = "DeleteUser")]
    [Authorize(Roles = "Owner")]
    public async Task<IActionResult> DeleteUser(long id)
    {
        await _userService.DeleteAsync(id);
        return NoContent();
    }
}
