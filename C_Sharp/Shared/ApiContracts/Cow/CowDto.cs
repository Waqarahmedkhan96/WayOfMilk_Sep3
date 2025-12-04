namespace ApiContracts.Cow;

// Create
public class CreateCowDto
{
    public required string RegNo { get; set; }
    public DateOnly BirthDate { get; set; }
    public bool IsHealthy { get; set; }
}
// Read
public class CowDto
{
    public int Id { get; set; }
    public required string RegNo { get; set; }
    public DateOnly BirthDate { get; set; }
    public bool IsHealthy { get; set; }
}


// Update
public class UpdateCowDto
{
    public required string RegNo { get; set; }
    public DateOnly BirthDate { get; set; }
    public bool IsHealthy { get; set; }
}

// Delete (batch)
public class DeleteCowsDto
{
    public required int[] Ids { get; set; }
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
    public bool? IsHealthy { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
}
