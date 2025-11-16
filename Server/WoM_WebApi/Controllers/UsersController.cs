using ApiContracts.Users;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using System.Linq;

namespace WebApi.Controllers;

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
        var user = new User { UserName = request.Name, Password = request.Password, Role = request.Role };
        var created = await _users.AddAsync(user);

        var dto = new UserDto { Id = created.Id, Name = created.UserName, Role = created.Role };
        return Created($"/users/{dto.Id}", dto);
    }

    // GET /users/{id}  // Read one
    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDto>> GetUser(int id)
    {
        var user = await _users.GetSingleAsync(id);
        var dto = new UserDto { Id = user.Id, Name = user.UserName, Role = user.Role };
        return Ok(dto);
    }

    // GET /users  // Read many
    [HttpGet]
    public ActionResult<IEnumerable<UserDto>> GetUsers([FromQuery] string? nameContains, [FromQuery] UserRole? role)
    {
        var query = _users.GetManyAsync();

        if (!string.IsNullOrWhiteSpace(nameContains))
            query = query.Where(u => u.UserName.Contains(nameContains, StringComparison.OrdinalIgnoreCase));
        if (role.HasValue)
            query = query.Where(u => u.Role == role.Value);

        var list = query.Select(u => new UserDto { Id = u.Id, Name = u.UserName, Role = u.Role }).ToList();
        return Ok(list);
    }

    // PUT /users/{id}  // Update
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto request)
    {
        var user = new User { Id = id, UserName = request.Name, Password = request.Password, Role = request.Role };
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
