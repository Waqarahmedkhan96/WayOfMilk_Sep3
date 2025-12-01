using ApiContracts.User;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WoM_WebApi.RestController;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _users;

    public UsersController(IUserRepository user)
    {
        this._users = user;
    }

    // POST /users  // Create
    [HttpPost]
    public async Task<ActionResult<UserDto>> AddUser([FromBody] CreateUserDto request)
    {
        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Password = request.Password,
            Role = request.Role
        };

        var created = await _users.AddAsync(user);

        var dto = new UserDto
        {
            Id = created.Id,
            Name = created.Name,
            Email = created.Email,
            PhoneNumber = created.PhoneNumber,
            Role = created.Role
        };

        return Created($"/users/{dto.Id}", dto);
    }

    // GET /users/{id}  // Read one
    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDto>> GetUser(int id)
    {
        var user = await _users.GetSingleAsync(id);

        var dto = new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Role = user.Role
        };

        return Ok(dto);
    }

    // GET /users  // Read many
    [HttpGet]
    public ActionResult<IEnumerable<UserDto>> GetUsers(
        [FromQuery] string? nameContains,
        [FromQuery] string? emailContains,
        [FromQuery] UserRole? role)
    {
        var query = _users.GetManyAsync();

        if (!string.IsNullOrWhiteSpace(nameContains))
            query = query.Where(u => u.Name.Contains(nameContains, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(emailContains))
            query = query.Where(u => u.Email.Contains(emailContains, StringComparison.OrdinalIgnoreCase));

        if (role.HasValue)
            query = query.Where(u => u.Role == role.Value);

        var list = query.Select(u => new UserDto
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email,
            PhoneNumber = u.PhoneNumber,
            Role = u.Role
        }).ToList();

        return Ok(list);
    }

    // PUT /users/{id}  // Update
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto request)
    {
        var user = new User
        {
            Id = id,
            Name = request.Name,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Password = request.Password,
            Role = request.Role
        };

        await _users.UpdateAsync(user);
        return NoContent();
    }

    // DELETE /users/{id}  // Delete
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        await _users.DeleteAsync(id);
        return NoContent();
    }
}
