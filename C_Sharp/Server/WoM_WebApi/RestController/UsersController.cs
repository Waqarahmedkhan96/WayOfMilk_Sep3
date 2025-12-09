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

    // ---------------------------------------
    // GET /users/current-user
    // ---------------------------------------
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

    // ---------------------------------------
    // GET /users (OWNER ONLY)
    // ---------------------------------------
    [HttpGet]
    [Authorize(Policy = "OwnerOnly")]
    public async Task<ActionResult<UserListDto>> GetAll()
    {
        var list = await _userService.GetAllAsync();
        return Ok(list);
    }

    // ---------------------------------------
    // GET /users/{id} (ANY LOGGED USER)
    // ---------------------------------------
    [HttpGet("{id:long}")]
    [Authorize]
    public async Task<ActionResult<UserDto>> GetById(long id)
    {
        var user = await _userService.GetByIdAsync(id);
        return Ok(user);
    }

    // ---------------------------------------
    // POST /users
    // PUBLIC SIGNUP
    // Vet NOT allowed here
    // ---------------------------------------
    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<UserDto>> CreateUser(CreateUserDto dto)
    {
        if (dto.Role == UserRole.Vet)
            return BadRequest("Vet accounts must be created by an Owner.");

        var created = await _userService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // ---------------------------------------
    // PUT /users/{id} (OWNER ONLY)
    // ---------------------------------------
    [HttpPut("{id:long}")]
    [Authorize(Policy = "OwnerOnly")]
    public async Task<ActionResult<UserDto>> UpdateUser(long id, UpdateUserDto dto)
    {
        dto.Id = id;
        var updated = await _userService.UpdateAsync(dto);
        return Ok(updated);
    }

    // ---------------------------------------
    // DELETE /users/{id} (OWNER ONLY)
    // ---------------------------------------
    [HttpDelete("{id:long}")]
    [Authorize(Policy = "OwnerOnly")]
    public async Task<IActionResult> DeleteUser(long id)
    {
        await _userService.DeleteAsync(id);
        return NoContent();
    }
}
