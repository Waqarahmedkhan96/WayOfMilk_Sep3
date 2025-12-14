// File: Server/WoM_WebApi/Mapping/MilkGrpcMapper.cs
using System;
using ApiContracts;
using Sep3.WayOfMilk.Grpc;

namespace WoM_WebApi.Mapping;

// Mapper: Milk DTOs ↔ gRPC
public static class MilkGrpcMapper
{
    // DateOnly → "yyyy-MM-dd"
    private static string ToGrpcDate(DateOnly date)
        => date.ToString("yyyy-MM-dd");

    // string → DateOnly (fallback value if empty)
    private static DateOnly FromGrpcDate(string? dateString)
        => string.IsNullOrWhiteSpace(dateString)
            ? new DateOnly(2000, 1, 1)
            : DateOnly.Parse(dateString);

    // ---------------- CREATE ----------------
    // DTO (create) → gRPC
    public static CreateMilkRequest ToGrpc(this CreateMilkDto dto)
        => new CreateMilkRequest
        {
            CowId = dto.CowId,
            ContainerId = dto.ContainerId,
            RegisteredByUserId = dto.RegisteredByUserId,
            Date = dto.Date.HasValue ? ToGrpcDate(dto.Date.Value) : string.Empty,
            VolumeL = dto.VolumeL,
            TestResult = dto.TestResult switch
            {
                MilkTestResult.Pass => MilkTestResultEnum.MilkTestPass,
                MilkTestResult.Fail => MilkTestResultEnum.MilkTestFail,
                _ => MilkTestResultEnum.MilkTestUnknown
            },

            // IMPORTANT: keep aligned with proto field "approvedForStorage"
            ApprovedForStorage = dto.ApprovedForStorage
        };

    // ---------------- UPDATE ----------------
    // DTO (update) + current → gRPC
    public static UpdateMilkRequest ToUpdateRequest(this UpdateMilkDto dto, MilkMessage current)
    {
        // 1) start from current values
        var result = new UpdateMilkRequest
        {
            Id = current.Id,
            ContainerId = current.ContainerId,
            Date = current.Date,
            VolumeL = current.VolumeL,
            TestResult = current.TestResult
        };

        // 2) apply changes only when provided
        if (dto.ContainerId.HasValue)
            result.ContainerId = dto.ContainerId.Value;

        if (dto.Date.HasValue)
            result.Date = ToGrpcDate(dto.Date.Value);

        if (dto.VolumeL.HasValue)
            result.VolumeL = dto.VolumeL.Value;

        if (dto.TestResult.HasValue)
        {
            result.TestResult = dto.TestResult.Value switch
            {
                MilkTestResult.Pass => MilkTestResultEnum.MilkTestPass,
                MilkTestResult.Fail => MilkTestResultEnum.MilkTestFail,
                MilkTestResult.Unknown => MilkTestResultEnum.MilkTestUnknown,
                _ => result.TestResult
            };
        }

        return result;
    }

    // ---------------- APPROVE ----------------
    // DTO (approve storage) → gRPC
    public static ApproveMilkStorageRequest ToGrpc(this ApproveMilkStorageDto dto)
        => new ApproveMilkStorageRequest
        {
            MilkId = dto.Id,
            ApprovedByUserId = dto.ApprovedByUserId,
            ApprovedForStorage = dto.ApprovedForStorage
        };

    // ---------------- READ ----------------
    // gRPC (single) → DTO
    public static MilkDto ToDto(this MilkMessage grpc)
        => new MilkDto
        {
            Id = grpc.Id,
            Date = FromGrpcDate(grpc.Date),
            VolumeL = grpc.VolumeL,
            TestResult = grpc.TestResult switch
            {
                MilkTestResultEnum.MilkTestPass => MilkTestResult.Pass,
                MilkTestResultEnum.MilkTestFail => MilkTestResult.Fail,
                _ => MilkTestResult.Unknown
            },
            CowId = grpc.CowId,
            ContainerId = grpc.ContainerId,
            RegisteredByUserId = grpc.RegisteredByUserId,
            ApprovedForStorage = grpc.ApprovedForStorage
        };

    // gRPC list → DTO list
    public static MilkListDto ToListDto(this MilkListReply grpc)
    {
        var result = new MilkListDto();
        foreach (var m in grpc.Milk)
            result.Milks.Add(m.ToDto());

        return result;
    }
}
