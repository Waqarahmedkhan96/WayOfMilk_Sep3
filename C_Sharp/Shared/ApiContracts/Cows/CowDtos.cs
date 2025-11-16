namespace ApiContracts.Cows;

// Read
public class CowDto
{
    public int Id { get; set; }
    public DateOnly BirthDate { get; set; }
}

// Create
public class CreateCowDto
{
    public DateOnly BirthDate { get; set; }
}

// Update
public class UpdateCowDto
{
    public DateOnly BirthDate { get; set; }
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
    public DateOnly? BornAfter { get; set; }
    public DateOnly? BornBefore { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
}
