using System;
using System.Collections.Generic;

namespace ApiContracts;

// DTO: create user (UI → WebApi)
public class CreateUserDto
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public required string Address { get; set; }
    public required string Password { get; set; }

    // IMPORTANT: enum, not string
    public UserRole Role { get; set; } = UserRole.Worker; 

    public string? LicenseNumber { get; set; } // for vets only
}

// DTO: single user (read)
public class UserDto
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public required string Address { get; set; }

    // enum here too
    public UserRole Role { get; set; }

    public string? LicenseNumber { get; set; }
}

// DTO: update user (no password)
public class UpdateUserDto
{
    public required long Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }

    // nullable enum (optional)
    public UserRole? Role { get; set; }
    public string? LicenseNumber { get; set; }
}

// DTO: delete users batch
public class DeleteUsersDto
{
    public required long[] Ids { get; set; }
}

// DTO: list of users
public class UserListDto
{
    public List<UserDto> Users { get; set; } = new();
}

// DTO: filter + paging
public class UserQueryParameters
{
    public string? NameContains { get; set; }
    public string? EmailContains { get; set; }
    public UserRole? Role { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
}


