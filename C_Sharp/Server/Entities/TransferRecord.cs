namespace Entities;

public class TransferRecord
{
    public int Id { get; set; }
    public DateOnly MovedAt { get; set; }
    public Department FromDept { get; set; }
    public Department ToDept { get; set; }
    public int RequestedByUserId { get; set; }
    public int? ApprovedByVetUserId { get; set; }
    public Department Department { get; set; }
    public int CowId { get; set; }
}
