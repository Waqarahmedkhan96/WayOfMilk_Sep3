namespace Entities;

public class Milk
{
    public int Id { get; set; }
    public DateOnly DateOnly { get; set; }
    public double VolumeL { get; set; }
    public MilkTestResult MilkTestResult { get; set; }
    public bool ApprovedForStorage { get; set; }

    public int CowId { get; set; }
    public int? ContainerId { get; set; }
    public int CreatedByUserId { get; set; }
}
