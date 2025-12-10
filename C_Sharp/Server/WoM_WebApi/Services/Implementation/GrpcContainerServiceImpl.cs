// File: Server/WoM_WebApi/Services/Implementation/GrpcContainerServiceImpl.cs
using ApiContracts;
using Sep3.WayOfMilk.Grpc;
using WoM_WebApi.GlobalExceptionHandler;
using WoM_WebApi.Mapping;
using WoM_WebApi.Services.Interfaces;

namespace WoM_WebApi.Services.Implementation;

// Impl: gRPC-based container service
public class GrpcContainerServiceImpl : IContainerService
{
    private readonly ContainerService.ContainerServiceClient _client;

    public GrpcContainerServiceImpl(ContainerService.ContainerServiceClient client)
    {
        _client = client;
    }

    // Create container
    public async Task<ContainerDto> CreateAsync(CreateContainerDto dto)
    {
        // 1) map DTO → gRPC
        var request = dto.ToGrpc();

        // 2) call Java create rpc
        var reply = await _client.CreateContainerAsync(request);

        if (reply == null || reply.Id == 0)
            throw new ValidationException("Unable to create container.");

        return reply.ToDto();
    }

    // Get container by id
    public async Task<ContainerDto> GetByIdAsync(long id)
    {
        // 1) call Java get rpc
        var reply = await _client.GetContainerAsync(new SentId { Id = id });

        // 2) validate existence
        if (reply == null || reply.Id == 0)
            throw new NotFoundException($"Container with id {id} not found.");

        return reply.ToDto();
    }

    // Get all containers
    public async Task<ContainerListDto> GetAllAsync()
    {
        // 1) call Java list rpc
        var reply = await _client.GetAllContainersAsync(new Empty());

        // 2) map list → DTO list
        return reply.ToListDto();
    }

    // Update container
    public async Task<ContainerDto> UpdateAsync(long id, UpdateContainerDto dto)
    {
        // 1) ensure container exists
        var existing = await _client.GetContainerAsync(new SentId { Id = id });
        if (existing == null || existing.Id == 0)
            throw new NotFoundException($"Container with id {id} not found.");

        // 2) map DTO → gRPC update
        var request = dto.ToGrpc(id);

        // 3) call Java update rpc
        var reply = await _client.UpdateContainerAsync(request);
        return reply.ToDto();
    }

    // Delete container
    public async Task DeleteAsync(long id)
    {
        // 1) ensure exists first
        var existing = await _client.GetContainerAsync(new SentId { Id = id });
        if (existing == null || existing.Id == 0)
            throw new NotFoundException($"Container with id {id} not found.");

        // 2) call delete rpc
        await _client.DeleteContainerAsync(new SentId { Id = id });
    }
}
