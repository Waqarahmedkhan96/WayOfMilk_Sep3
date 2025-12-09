using System;
using System.Collections.Generic;

namespace ApiContracts;

// DTO: create cow (UI → WebApi)
public class CreateCowDto
{
    public required string RegNo { get; set; }     // proto: regNo
    public DateOnly BirthDate { get; set; }        // proto: birthDate string
    public long RegisteredByUserId { get; set; }   // who registered
    public long DepartmentId { get; set; }         // quarantine dept id
}

// DTO: single cow (read)
public class CowDto
{
    public long Id { get; set; }
    public required string RegNo { get; set; }
    public DateOnly BirthDate { get; set; }
    public bool Healthy { get; set; }              // proto: isHealthy
    public long? DepartmentId { get; set; }        // proto: departmentId
    public string? DepartmentName { get; set; }    // UI-only enrichment
}

// DTO: update cow
public class UpdateCowDto
{
    public required long Id { get; set; }
    public string? RegNo { get; set; }
    public DateOnly? BirthDate { get; set; }
    public bool? Healthy { get; set; }
    public long? DepartmentId { get; set; }
}

// DTO: delete cows batch
public class DeleteCowsDto
{
    public required long[] Ids { get; set; }
}

// DTO: list of cows
public class CowListDto
{
    public List<CowDto> Cows { get; set; } = new();
}

// DTO: cow filters
public class CowQueryParameters
{
    public string? RegNoEquals { get; set; }
    public DateOnly? BornAfter { get; set; }
    public DateOnly? BornBefore { get; set; }
    public bool? Healthy { get; set; }
    public long? DepartmentId { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
}
