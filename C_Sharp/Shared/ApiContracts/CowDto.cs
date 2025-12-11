using System;
using System.Collections.Generic;

namespace ApiContracts;

// 1. CREATE
// Kept separate because creating often requires different fields than reading (e.g., you don't have an ID yet).
public class CowCreationDto
{
    public required string RegNo { get; set; }
    public DateOnly BirthDate { get; set; }
    public long? DepartmentId { get; set; }
    public long RegisteredByUserId { get; set; }
}
//same function but in case someone is confused about the naming
public class CreateCowDto
{
    public required string RegNo { get; set; }     // proto: regNo
    public DateOnly BirthDate { get; set; }        // proto: birthDate string
    public long RegisteredByUserId { get; set; }   // who registered
    public long DepartmentId { get; set; }         // quarantine dept id
}

// DTO: single cow (read)
// This matches 'CowData' proto message
public class CowDto
{
    public long Id { get; set; }
    public required string RegNo { get; set; }
    public DateOnly BirthDate { get; set; }
    public bool Healthy { get; set; }              // proto: isHealthy
    public long? DepartmentId { get; set; }        // proto: departmentId
    public string? DepartmentName { get; set; }    // UI-only enrichment
}

// Update
//not really what we use in the proto for this, however
public class UpdateCowDto
{
    public required long Id { get; set; }
    public string? RegNo { get; set; }
    public DateOnly? BirthDate { get; set; }
    public bool? Healthy { get; set; }
    public long? DepartmentId { get; set; }
}


// QUERY PARAMETERS (filters)
// Excellent practice to have this. It keeps your controller signatures clean.
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

public class UpdateHealthRequest
{
    public required IEnumerable<long> CowIds { get; set; }
    public bool NewHealthStatus { get; set; }
}


// REMOVED: DeleteCowsDto
// REMOVED: CowListDto
// Instead of wrapping the list in an object,
// simply return IEnumerable<CowDto> or List<CowDto>