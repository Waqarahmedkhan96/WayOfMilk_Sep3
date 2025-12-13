// File: Server/WoM_WebApi/Mapping/ContainerGrpcMapper.cs
using ApiContracts;
using Sep3.WayOfMilk.Grpc;

namespace WoM_WebApi.Mapping;

// Mapper: Container DTOs ↔ gRPC
public static class ContainerGrpcMapper
{
    // DTO (create) → gRPC
    public static CreateContainerRequest ToGrpc(this CreateContainerDto dto)
        => new CreateContainerRequest
        {
            CapacityL = dto.CapacityL
        };

    // DTO (update) → gRPC
    public static UpdateContainerRequest ToGrpc(this UpdateContainerDto dto, long id)
        => new UpdateContainerRequest
        {
            Id = id,
            CapacityL = dto.CapacityL
        };

    // gRPC → DTO
    public static ContainerDto ToDto(this ContainerMessage grpc)
        => new ContainerDto
        {
            Id = grpc.Id,
            CapacityL = grpc.CapacityL,
            OccupiedCapacityL = grpc.OccupiedCapacityL
        };

    // gRPC list → DTO list
    public static ContainerListDto ToListDto(this ContainerListReply grpc)
    {
        var result = new ContainerListDto();
        foreach (var c in grpc.Containers)
        {
            result.Containers.Add(c.ToDto());
        }
        return result;
    }
}
