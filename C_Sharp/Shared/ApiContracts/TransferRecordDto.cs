namespace ApiContracts;

// Create (user-requested transfer)
public class CreateTransferRecordDto
{
    public DateOnly? MovedAt { get; set; } // null => backend may set today
    public long FromDepartmentId { get; set; }
    public long ToDepartmentId { get; set; }
    public long RequestedByUserId { get; set; }
    public long CowId { get; set; }
}

// Read
public class TransferRecordDto
{
    public long Id { get; set; }
    public DateOnly MovedAt { get; set; }
    public long CowId { get; set; }
    public long FromDepartmentId { get; set; }
    public long ToDepartmentId { get; set; }
    public long CurrentDepartmentId { get; set; }
    public long RequestedByUserId { get; set; }
    public long? ApprovedByVetUserId { get; set; }
}

// Update (if you ever need to edit)
public class UpdateTransferRecordDto
{
    public DateOnly MovedAt { get; set; }
    public long FromDepartmentId { get; set; }
    public long ToDepartmentId { get; set; }
    public long CurrentDepartmentId { get; set; }
    public long RequestedByUserId { get; set; }
    public long? ApprovedByVetUserId { get; set; }
    public long CowId { get; set; }
}

// Action: approve
public class ApproveTransferDto
{
    public long VetUserId { get; set; }
    public long ToDepartmentId { get; set; } // used in controller
}

// Delete (batch)
public class DeleteTransfersDto
{
    public required long[] Ids { get; set; }
}

// List
public class TransferRecordListDto
{
    public List<TransferRecordDto> Transfers { get; set; } = new();
}

// Query
public class TransferRecordQueryParameters
{
    public long? CowId { get; set; }
    public long? DepartmentId { get; set; } // filter by current dept
    public DateOnly? FromMovedAt { get; set; }
    public DateOnly? ToMovedAt { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
}
