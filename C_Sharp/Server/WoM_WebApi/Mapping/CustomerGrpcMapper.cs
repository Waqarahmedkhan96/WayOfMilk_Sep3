// File: Server/WoM_WebApi/Mapping/CustomerGrpcMapper.cs
using ApiContracts;
using Sep3.WayOfMilk.Grpc;

namespace WoM_WebApi.Mapping;

// Mapper: Customer DTOs ↔ gRPC
public static class CustomerGrpcMapper
{
    // DTO (create) → gRPC
    public static CustomerCreationRequest ToGrpc(this CreateCustomerDto dto)
        => new CustomerCreationRequest
        {
            CompanyName = dto.CompanyName,
            PhoneNo = dto.PhoneNo,
            Email = dto.Email,
            CompanyCVR = dto.CompanyCVR
        };

    // gRPC → DTO
    public static CustomerDto ToDto(this CustomerData grpc)
        => new CustomerDto
        {
            Id = grpc.Id,
            CompanyName = grpc.CompanyName ?? string.Empty,
            PhoneNo = grpc.PhoneNo ?? string.Empty,
            Email = grpc.Email ?? string.Empty,
            CompanyCVR = grpc.CompanyCVR ?? string.Empty
        };

    // gRPC list → DTO list
    public static CustomerListDto ToListDto(this CustomerList grpc)
    {
        var result = new CustomerListDto();
        foreach (var c in grpc.Customers)
        {
            result.Customers.Add(c.ToDto());
        }
        return result;
    }
}
