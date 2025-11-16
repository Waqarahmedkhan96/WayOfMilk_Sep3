namespace ApiContracts.Milk;

using Entities; // enums

// Read
public class MilkDto
{
    public int Id { get; set; }
    public DateOnly DateOnly { get; set; }
    public double VolumeL { get; set; }
    public MilkTestResult MilkTestResult { get; set; }
    public bool ApprovedForStorage { get; set; }
    public int CowId { get; set; }
    public int? ContainerId { get; set; }
}

// Create
public class CreateMilkDto
{
    public DateOnly DateOnly { get; set; }
    public double VolumeL { get; set; }
    public int CowId { get; set; }
    public int? ContainerId { get; set; }

    // for approval by worker while adding
    public bool ApprovedForStorage { get; set; }    // approve?
    public MilkTestResult MilkTestResult { get; set; } // must be pass when approving

}

// Update
public class UpdateMilkDto
{
    public DateOnly DateOnly { get; set; }
    public double VolumeL { get; set; }
    public MilkTestResult MilkTestResult { get; set; }
    public bool ApprovedForStorage { get; set; }
    public int CowId { get; set; }
    public int? ContainerId { get; set; }
}

// Action: approve
public class ApproveMilkDto
{
    public bool ApprovedForStorage { get; set; }
    public MilkTestResult MilkTestResult { get; set; }
}

// Delete (batch)
public class DeleteMilksDto
{
    public required int[] Ids { get; set; }
}

// List
public class MilkListDto
{
    public List<MilkDto> Milks { get; set; } = new();
}

// Query
public class MilkQueryParameters
{
    public int? CowId { get; set; }
    public int? ContainerId { get; set; }
    public DateOnly? From { get; set; }
    public DateOnly? To { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
}
