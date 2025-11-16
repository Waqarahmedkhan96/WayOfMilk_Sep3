namespace ApiContracts.Users;

using Entities; // enums

// Read
public class UserDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public UserRole Role { get; set; }
}

// Create
public class CreateUserDto
{
    public required string Name { get; set; }
    public required string Password { get; set; }  // never return this
    public UserRole Role { get; set; }
}

// Update
public class UpdateUserDto
{
    public required string Name { get; set; }
    public required string Password { get; set; }
    public UserRole Role { get; set; }
}

// Delete (batch)
public class DeleteUsersDto
{
    public required int[] Ids { get; set; }
}

// List
public class UserListDto
{
    public List<UserDto> Users { get; set; } = new();
}

// Query
public class UserQueryParameters
{
    public string? NameContains { get; set; }
    public UserRole? Role { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
}
