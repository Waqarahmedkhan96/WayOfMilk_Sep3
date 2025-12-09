namespace ApiContracts;

// Create
public class CreateCowDto
{
    public required string RegNo { get; set; }
    public DateOnly BirthDate { get; set; }
    public bool Healthy { get; set; }
    public long? DepartmentId { get; set; } // optional initial dept
    public long RegisteredByUserId { get; set; } // who registered
}

// Read
public class CowDto
{
    public long Id { get; set; }
    public required string RegNo { get; set; }
    public DateOnly BirthDate { get; set; }
    public bool Healthy { get; set; }
    public long? DepartmentId { get; set; }
    public string? DepartmentName { get; set; }
}

// Update
public class UpdateCowDto
{
    public required string RegNo { get; set; }
    public DateOnly BirthDate { get; set; }
    public bool Healthy { get; set; }
    public long? DepartmentId { get; set; }
}

// Delete (batch)
public class DeleteCowsDto
{
    public required long[] Ids { get; set; }
}

// List
public class CowListDto
{
    public List<CowDto> Cows { get; set; } = new();
}

// Query
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
