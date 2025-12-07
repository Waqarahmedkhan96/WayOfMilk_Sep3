namespace ApiContracts;

// Create
public class CreateMilkDto
{
    public DateOnly? Date { get; set; } // null => Java uses today
    public double VolumeL { get; set; }
    public long CowId { get; set; }
    public long ContainerId { get; set; }
    public MilkTestResult MilkTestResult { get; set; }
    public long RegisteredByUserId { get; set; }
}

// Read
public class MilkDto
{
    public long Id { get; set; }
    public DateOnly Date { get; set; }
    public double VolumeL { get; set; }
    public MilkTestResult MilkTestResult { get; set; }
    public long CowId { get; set; }
    public long ContainerId { get; set; }
    public long RegisteredByUserId { get; set; }
}

// Update
public class UpdateMilkDto
{
    public DateOnly Date { get; set; }
    public double VolumeL { get; set; }
    public MilkTestResult MilkTestResult { get; set; }
    public long CowId { get; set; }
    public long ContainerId { get; set; }
}

// Action: approve (if you still want a UI action)
public class ApproveMilkDto
{
    public MilkTestResult MilkTestResult { get; set; }
}

// Delete (batch)
public class DeleteMilksDto
{
    public required long[] Ids { get; set; }
}

// List
public class MilkListDto
{
    public List<MilkDto> Milks { get; set; } = new();
}

// Query
public class MilkQueryParameters
{
    public long? CowId { get; set; }
    public long? ContainerId { get; set; }
    public DateOnly? From { get; set; }
    public DateOnly? To { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
}
