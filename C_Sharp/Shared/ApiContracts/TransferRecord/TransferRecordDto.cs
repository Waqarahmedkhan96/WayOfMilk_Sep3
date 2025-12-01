namespace ApiContracts.TransferRecord;

using Entities;

// Create
public class CreateTransferRecordDto
{
    public Department Department { get; set; }
    public DateOnly MovedAt { get; set; }
    public Department FromDept { get; set; }
    public Department ToDept { get; set; }
    public int RequestedByUserId { get; set; }
    public int CowId { get; set; }
}

// Read
public class TransferRecordDto
{
    public int Id { get; set; }
    public Department Department { get; set; }
    public DateOnly MovedAt { get; set; }
    public Department FromDept { get; set; }
    public Department ToDept { get; set; }
    public int RequestedByUserId { get; set; }
    public int? ApprovedByVetUserId { get; set; }
    public int CowId { get; set; }
}

// Update
public class UpdateTransferRecordDto
{
    public Department Department { get; set; }
    public DateOnly MovedAt { get; set; }
    public Department FromDept { get; set; }
    public Department ToDept { get; set; }
    public int RequestedByUserId { get; set; }
    public int? ApprovedByVetUserId { get; set; }
    public int CowId { get; set; }
}

// Action: approve
public class ApproveTransferDto
{
    public int VetUserId { get; set; }
}

// Delete (batch)
public class DeleteTransfersDto
{
    public required int[] Ids { get; set; }
}

// List
public class TransferRecordListDto
{
    public List<TransferRecordDto> Transfers { get; set; } = new();
}

// Query
public class TransferRecordQueryParameters
{
    public int? CowId { get; set; }
    public Department? Department { get; set; }
    public DateOnly? FromMovedAt { get; set; }
    public DateOnly? ToMovedAt { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
}