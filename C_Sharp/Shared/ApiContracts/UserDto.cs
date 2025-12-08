namespace ApiContracts;

// Create
public class CreateUserDto
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Address { get; set; }
    public required string Password { get; set; }
    public UserRole Role { get; set; } = UserRole.Worker; // default worker
    public string? VetLicenseNo { get; set; } // only for VET
}

// Read
public class UserDto
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Address { get; set; }
    public UserRole Role { get; set; }
    public string? VetLicenseNo { get; set; }
}

// Update (no password here – password handled in Auth)
//needs id, however, set up automatically, and everything else is optional
public class UpdateUserDto
{
    public required long Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public UserRole? Role { get; set; }
    public string? VetLicenseNo { get; set; }
}

// Delete (batch)
public class DeleteUsersDto
{
    public required long[] Ids { get; set; }
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
    public string? EmailContains { get; set; }
    public UserRole? Role { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
}
