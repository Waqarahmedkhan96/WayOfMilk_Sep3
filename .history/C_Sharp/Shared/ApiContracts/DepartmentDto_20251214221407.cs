using System;
using System.Collections.Generic;

namespace ApiContracts;

// DTO: create department
public class CreateDepartmentDto
{
    public DepartmentType Type { get; set; } // QUARANTINE/MILKING/RESTING
}

// DTO: single department
public class DepartmentDto
{
    public long Id { get; set; }
    public DepartmentType Type { get; set; }
    public string Name { get; set; } = string.Empty;
}

// DTO: update department
public class UpdateDepartmentDto
{
    public DepartmentType Type { get; set; }
    public string Name { get; set; } = string.Empty;
}

// DTO: delete departments batch
public class DeleteDepartmentsDto
{
    public required long[] Ids { get; set; }
}

// DTO: list of departments
public class DepartmentListDto
{
    public List<DepartmentDto> Departments { get; set; } = new();
}

// DTO: department filters
public class DepartmentQueryParameters
{
    public DepartmentType? Type { get; set; }
    public string? NameContains { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
}
