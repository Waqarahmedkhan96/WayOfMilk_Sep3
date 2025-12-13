// File: Server/WoM_WebApi/Mapping/SaleGrpcMapper.cs
using System;
using ApiContracts;
using Sep3.WayOfMilk.Grpc;

namespace WoM_WebApi.Mapping;

// Mapper: Sale DTOs ↔ gRPC
public static class SaleGrpcMapper
{
    // helper: DateTime → ISO string
    private static string ToGrpcDateTime(DateTime dt)
        => dt.ToString("yyyy-MM-dd'T'HH:mm:ss");

    // helper: string → DateTime
    private static DateTime FromGrpcDateTime(string? s)
        => string.IsNullOrWhiteSpace(s)
            ? DateTime.MinValue
            : DateTime.Parse(s);

    // DTO (create) → gRPC
    public static SaleCreationRequest ToGrpc(this CreateSaleDto dto)
        => new SaleCreationRequest
        {
            CustomerId = dto.CustomerId,
            ContainerId = dto.ContainerId,
            QuantityL = dto.QuantityL,
            Price = dto.Price,
            CreatedByUserId = dto.CreatedByUserId,
            DateTime = ToGrpcDateTime(dto.DateTime ?? DateTime.UtcNow),
            RecallCase = dto.RecallCase
        };

    // gRPC → DTO
    public static SaleDto ToDto(this SaleData grpc)
        => new SaleDto
        {
            Id = grpc.Id,
            CustomerId = grpc.CustomerId,
            ContainerId = grpc.ContainerId,
            QuantityL = grpc.QuantityL,
            Price = grpc.Price,
            DateTime = FromGrpcDateTime(grpc.DateTime),
            RecallCase = grpc.RecallCase,
            CreatedByUserId = grpc.CreatedByUserId
        };

    // gRPC list → DTO list
    public static SaleListDto ToListDto(this SaleList grpc)
    {
        var result = new SaleListDto();
        foreach (var s in grpc.Sales)
        {
            result.Sales.Add(s.ToDto());
        }
        return result;
    }
}
