// File: Server/WoM_WebApi/Mapping/TransferRecordGrpcMapper.cs
using System;
using ApiContracts;
using Sep3.WayOfMilk.Grpc;

namespace WoM_WebApi.Mapping;

// Mapper: TransferRecord DTOs ↔ gRPC
public static class TransferRecordGrpcMapper
{
    // helper: DateTime → ISO string          // C# DateTime → proto string
    private static string ToGrpcDateTime(DateTime dt)
        => dt.ToString("yyyy-MM-dd'T'HH:mm:ss");

    // helper: string → DateTime              // proto string → C# DateTime
    private static DateTime FromGrpcDateTime(string? s)
        => string.IsNullOrWhiteSpace(s)
            ? DateTime.MinValue
            : DateTime.Parse(s);

    // DTO (create) → gRPC
    public static TransferRecordCreationRequest ToGrpc(this CreateTransferRecordDto dto)
        => new TransferRecordCreationRequest
        {
            CowId = dto.CowId,
            FromDepartmentId = dto.FromDepartmentId,
            ToDepartmentId = dto.ToDepartmentId,
            RequestedByUserId = dto.RequestedByUserId,
            MovedAt = dto.MovedAt.HasValue ? ToGrpcDateTime(dto.MovedAt.Value) : string.Empty
        };

    // DTO (approve) → gRPC
    public static ApproveTransferRequest ToGrpc(this ApproveTransferDto dto)
        => new ApproveTransferRequest
        {
            TransferId = dto.TransferId,
            VetUserId = dto.VetUserId
        };

    // DTO (update) + current → gRPC
    public static TransferRecordData ToUpdateRecord(this UpdateTransferRecordDto dto, TransferRecordData current)
    {
        // 1) copy current state                // baseline from DB
        var result = new TransferRecordData
        {
            Id = current.Id,
            MovedAt = current.MovedAt,
            FromDepartmentId = current.FromDepartmentId,
            ToDepartmentId = current.ToDepartmentId,
            DepartmentId = current.DepartmentId,
            RequestedByUserId = current.RequestedByUserId,
            ApprovedByVetUserId = current.ApprovedByVetUserId,
            CowId = current.CowId
        };

        // 2) override from DTO when not null   // apply incoming changes
        if (dto.MovedAt.HasValue)
            result.MovedAt = ToGrpcDateTime(dto.MovedAt.Value);

        if (dto.FromDepartmentId.HasValue)
            result.FromDepartmentId = dto.FromDepartmentId.Value;

        if (dto.ToDepartmentId.HasValue)
            result.ToDepartmentId = dto.ToDepartmentId.Value;

        if (dto.DepartmentId.HasValue)
            result.DepartmentId = dto.DepartmentId.Value;

        if (dto.RequestedByUserId.HasValue)
            result.RequestedByUserId = dto.RequestedByUserId.Value;

        if (dto.ApprovedByVetUserId.HasValue)
            result.ApprovedByVetUserId = dto.ApprovedByVetUserId.Value;

        if (dto.CowId.HasValue)
            result.CowId = dto.CowId.Value;

        return result;
    }

    // gRPC → DTO
    public static TransferRecordDto ToDto(this TransferRecordData grpc)
        => new TransferRecordDto
        {
            Id = grpc.Id,
            MovedAt = FromGrpcDateTime(grpc.MovedAt),
            CowId = grpc.CowId,
            FromDepartmentId = grpc.FromDepartmentId == 0 ? null : grpc.FromDepartmentId,
            ToDepartmentId = grpc.ToDepartmentId == 0 ? null : grpc.ToDepartmentId,
            DepartmentId = grpc.DepartmentId,
            RequestedByUserId = grpc.RequestedByUserId,
            ApprovedByVetUserId = grpc.ApprovedByVetUserId == 0 ? null : grpc.ApprovedByVetUserId
        };

    // gRPC list → DTO list
    public static TransferRecordListDto ToListDto(this TransferRecordList grpc)
    {
        var result = new TransferRecordListDto();
        foreach (var r in grpc.Records)
        {
            result.Transfers.Add(r.ToDto());
        }
        return result;
    }
}
