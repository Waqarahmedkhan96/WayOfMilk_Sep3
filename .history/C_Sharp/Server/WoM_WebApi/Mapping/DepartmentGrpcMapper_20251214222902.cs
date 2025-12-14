// File: Server/WoM_WebApi/Mapping/DepartmentGrpcMapper.cs
using ApiContracts;
using Sep3.WayOfMilk.Grpc;

namespace WoM_WebApi.Mapping;

// Mapper: Department DTOs ↔ gRPC
public static class DepartmentGrpcMapper
{
    // enum: DTO → string
    private static string ToGrpcString(DepartmentType type)
        => type switch
        {
            DepartmentType.Quarantine => "QUARANTINE",
            DepartmentType.Milking => "MILKING",
            DepartmentType.Resting => "RESTING",
            _ => "RESTING"
        };

    // string → DTO
    private static DepartmentType FromGrpcString(string? s)
        => (s ?? "RESTING").ToUpperInvariant() switch
        {
            "QUARANTINE" => DepartmentType.Quarantine,
            "MILKING" => DepartmentType.Milking,
            "RESTING" => DepartmentType.Resting,
            _ => DepartmentType.Resting
        };

    // DTO (create) → gRPC
    public static DepartmentCreationRequest ToGrpc(this CreateDepartmentDto dto)
        => new DepartmentCreationRequest
        {
            Type = ToGrpcString(dto.Type),
            Name = dto.Name
        };

    // DTO (update) → gRPC
    public static DepartmentData ToGrpc(this UpdateDepartmentDto dto, long id)
        => new DepartmentData
        {
            Id = id,
            Type = ToGrpcString(dto.Type),
            Name = dto.Name
        };

    // gRPC → DTO
    public static DepartmentDto ToDto(this DepartmentData grpc)
        => new DepartmentDto
        {
            Id = grpc.Id,
            Type = FromGrpcString(grpc.Type), 
            Name = grpc.Name
        };

    // gRPC list → DTO list
    public static DepartmentListDto ToListDto(this DepartmentList grpc)
    {
        var result = new DepartmentListDto();
        foreach (var d in grpc.Departments)
        {
            result.Departments.Add(d.ToDto());
        }
        return result;
    }
}
