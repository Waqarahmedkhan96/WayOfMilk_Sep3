using System;
using System.Collections.Generic;

namespace ApiContracts;

// DTO: create milk record
public class CreateMilkDto
{
    public DateOnly? Date { get; set; }              // null => use today
    public double VolumeL { get; set; }
    public MilkTestResult TestResult { get; set; }   // proto enum
    public long CowId { get; set; }
    public long ContainerId { get; set; }
    public long RegisteredByUserId { get; set; }     // proto: registeredByUserId
    // NEW: what the user *wants* as final approval state
    public bool ApprovedForStorage { get; set; } = false;
}

// DTO: single milk record
public class MilkDto
{
    public long Id { get; set; }
    public DateOnly Date { get; set; }
    public double VolumeL { get; set; }
    public MilkTestResult TestResult { get; set; }
    public long CowId { get; set; }
    public long ContainerId { get; set; }
    public long RegisteredByUserId { get; set; }
    public bool ApprovedForStorage { get; set; }     // proto: approvedForStorage
}

// DTO: update milk record
public class UpdateMilkDto
{
    public long Id { get; set; }
    public DateOnly? Date { get; set; }
    public double? VolumeL { get; set; }
    public MilkTestResult? TestResult { get; set; }
    public long? ContainerId { get; set; }
     // NEW: optional – if provided, we will approve/deny
    public bool? ApprovedForStorage { get; set; }
}

// DTO: approve/deny storage
public class ApproveMilkStorageDto
{
    public long Id { get; set; }                 // milk id
    public long ApprovedByUserId { get; set; }   // vet/owner id
    public bool ApprovedForStorage { get; set; } // true/false
}

// DTO: delete milk batch
public class DeleteMilksDto
{
    public required long[] Ids { get; set; }
}

// DTO: list of milk records
public class MilkListDto
{
    public List<MilkDto> Milks { get; set; } = new();
}

// DTO: milk filters
public class MilkQueryParameters
{
    public long? CowId { get; set; }
    public long? ContainerId { get; set; }
    public DateOnly? From { get; set; }
    public DateOnly? To { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
}
