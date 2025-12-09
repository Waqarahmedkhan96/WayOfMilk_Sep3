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

// 2. READ (The standard "Cow")
// This matches 'CowData' proto message
public class CowDto
{
    public long Id { get; set; }
    public required string RegNo { get; set; }
    public DateOnly BirthDate { get; set; }
    public bool Healthy { get; set; }
    public long? DepartmentId { get; set; }
    public string? DepartmentName { get; set; }
}

// 3. UPDATE
// Kept separate for now just in case we need something without ID
//although the t3 will never update and id
public class UpdateCowDto
{
    public required string RegNo { get; set; }
    public DateOnly BirthDate { get; set; }
    public bool Healthy { get; set; }
    public long? DepartmentId { get; set; }
    public required long RegisteredByUserId { get; set; }
}

// 4. QUERY PARAMETERS
// Excellent practice to keep this. It keeps your controller signatures clean.
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