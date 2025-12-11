// File: Server/WoM_WebApi/Mapping/CowGrpcMapper.cs
using System;
using ApiContracts;
using Sep3.WayOfMilk.Grpc;

namespace WoM_WebApi.Mapping;

// Mapper: Cow DTOs ↔ gRPC
public static class CowGrpcMapper
{
    // helper: DateOnly → "yyyy-MM-dd"
    private static string ToGrpcDate(DateOnly date)
        => date.ToString("yyyy-MM-dd");

    // helper: string → DateOnly (safe)
    private static DateOnly FromGrpcDate(string? dateString)
        => string.IsNullOrWhiteSpace(dateString)
            ? new DateOnly(2000, 1, 1)
            : DateOnly.Parse(dateString);

    // DTO (create) → gRPC request
    public static CowCreationRequest ToGrpc(this CreateCowDto dto)
        => new CowCreationRequest
        {
            RegNo = dto.RegNo,
            BirthDate = ToGrpcDate(dto.BirthDate),
            RegisteredByUserId = dto.RegisteredByUserId,
            QuarantineDepartmentId = dto.DepartmentId // always quarantine at start
        };

    // gRPC (single cow) → DTO
    public static CowDto ToDto(this CowData grpc)
        => new CowDto
        {
            Id = grpc.Id,
            RegNo = grpc.RegNo ?? string.Empty,
            BirthDate = FromGrpcDate(grpc.BirthDate),
            Healthy = grpc.IsHealthy,                 // bool from proto
            DepartmentId = grpc.DepartmentId == 0 ? null : grpc.DepartmentId,
            DepartmentName = null                    // can be filled from DepartmentService in UI
        };

    // gRPC list → DTO list
    /*
    public static CowListDto ToListDto(this CowList grpc)
    {
        var result = new CowListDto();
        foreach (var c in grpc.Cows)
        {
            result.Cows.Add(c.ToDto());
        }
        return result;
    }
    */

    // DTO (update) + current cowData → CowUpdateRequest
    public static CowUpdateRequest ToUpdateRequest(
        this UpdateCowDto dto,
        CowData current,
        long requestedByUserId)
    {
        // 1) copy current state         // copy existing state
        var cowData = new CowData
        {
            Id = current.Id,
            RegNo = current.RegNo,
            BirthDate = current.BirthDate,
            IsHealthy = current.IsHealthy,
            DepartmentId = current.DepartmentId
        };

        // 2) override only values present in DTO         // apply incoming changes
        if (dto.RegNo is not null)
            cowData.RegNo = dto.RegNo;

        if (dto.BirthDate.HasValue)
            cowData.BirthDate = ToGrpcDate(dto.BirthDate.Value);

        if (dto.Healthy.HasValue)
            cowData.IsHealthy = dto.Healthy.Value;

        if (dto.DepartmentId.HasValue)
            cowData.DepartmentId = dto.DepartmentId.Value;

        // 3) wrap into request with requestedByUserId   // build final request
        return new CowUpdateRequest
        {
            CowData = cowData,
            RequestedBy = requestedByUserId
        };
    }
}
