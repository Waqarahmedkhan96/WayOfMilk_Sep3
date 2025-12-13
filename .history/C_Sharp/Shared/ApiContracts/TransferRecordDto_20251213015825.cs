using System;
using System.Collections.Generic;

namespace ApiContracts;

// DTO: create transfer record
public class CreateTransferRecordDto
{
    public DateTime? MovedAt { get; set; }  // if null → API layer sets UtcNow before gRPC call (Java requires movedAt) , Why both DateTime?
    public long FromDepartmentId { get; set; }
    public long ToDepartmentId { get; set; }
    public long RequestedByUserId { get; set; }
    public long CowId { get; set; }
}

// DTO: single transfer record
public class TransferRecordDto
{
    public long Id { get; set; }
    public DateTime MovedAt { get; set; }
    public long CowId { get; set; }

    public long? FromDepartmentId { get; set; }
    public long? ToDepartmentId { get; set; }

    public long RequestedByUserId { get; set; }
    public long? ApprovedByVetUserId { get; set; }
}

// DTO: update transfer record (rarely used)
public class UpdateTransferRecordDto
{
    public long Id { get; set; }
    public DateTime? MovedAt { get; set; }
    public long? FromDepartmentId { get; set; }
    public long? ToDepartmentId { get; set; }
    public long? RequestedByUserId { get; set; }
    public long? ApprovedByVetUserId { get; set; }
    public long? CowId { get; set; }
}

// DTO: approve transfer (vet)
public class ApproveTransferDto
{
    public long TransferId { get; set; }   // proto: transferId
    public long VetUserId { get; set; }    // proto: vetUserId
}

// DTO: delete transfers batch
public class DeleteTransfersDto
{
    public required long[] Ids { get; set; }
}

// DTO: list of transfer records
public class TransferRecordListDto
{
    public List<TransferRecordDto> Transfers { get; set; } = new();
}

// DTO: transfer filters
public class TransferRecordQueryParameters
{
    public long? CowId { get; set; }
    public DateTime? MovedAt { get; set; }
    public long? FromDepartmentId { get; set; }
    public long? ToDepartmentId { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
}
