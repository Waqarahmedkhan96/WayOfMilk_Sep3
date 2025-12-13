using System;
using System.Collections.Generic;

namespace ApiContracts;

// DTO: create department
public class CreateDepartmentDto
{
    public DepartmentType Type { get; set; }
    public string Name { get; set; } = string.Empty;
}


// DTO: single department
public class DepartmentDto
{
    public long Id { get; set; }
    public DepartmentType Type { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<CowDto> Cows { get; set; } = new List<CowDto>();
    public ICollection<TransferRecordDto> TransfersFrom { get; set; } = new List<TransferRecordDto>();
    public ICollection<TransferRecordDto> TransfersTo { get; set; } = new List<TransferRecordDto>();
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
    public int? Page { get; set; }
    public int? PageSize { get; set; }
}
