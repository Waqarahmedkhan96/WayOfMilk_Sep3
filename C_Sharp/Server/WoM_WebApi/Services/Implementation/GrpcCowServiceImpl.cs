// File: Server/WoM_WebApi/Services/Implementation/GrpcCowServiceImpl.cs
using ApiContracts;
using Sep3.WayOfMilk.Grpc;
using WoM_WebApi.GlobalExceptionHandler;
using WoM_WebApi.Mapping;
using WoM_WebApi.Services.Interfaces;

namespace WoM_WebApi.Services.Implementation;

// Impl: gRPC-based cow service
public class GrpcCowServiceImpl : ICowService
{
    private readonly CowService.CowServiceClient _client;

    public GrpcCowServiceImpl(CowService.CowServiceClient client)
    {
        _client = client;
    }

    // Create new cow
    public async Task<CowDto> CreateAsync(CreateCowDto dto, long requestedByUserId)
    {
        // 1) map DTO → gRPC request
        var request = dto.ToGrpc();

        // 2) call Java gRPC service
        var reply = await _client.AddCowAsync(request);

        if (reply == null || reply.Id == 0)
            throw new ValidationException("Unable to create cow.");

        return reply.ToDto();
    }

    // Get cow by id
    public async Task<CowDto> GetByIdAsync(long id)
    {
        // 1) call Java with id
        var reply = await _client.GetCowByIdAsync(new CowIdRequest { Id = id });

        // 2) validate existence
        if (reply == null || reply.Id == 0)
            throw new NotFoundException($"Cow with id {id} not found.");

        return reply.ToDto();
    }

    // Get all cows
    public async Task<CowListDto> GetAllAsync()
    {
        // 1) call Java list rpc
        var reply = await _client.GetAllCowsAsync(new Empty());

        // 2) map list → DTO list
        return reply.ToListDto();
    }

    // Update existing cow
    public async Task<CowDto> UpdateAsync(UpdateCowDto dto, long requestedByUserId)
    {
        // 1) fetch current cow from Java
        var current = await _client.GetCowByIdAsync(new CowIdRequest { Id = dto.Id });

        if (current == null || current.Id == 0)
            throw new NotFoundException($"Cow with id {dto.Id} not found.");

        // 2) map DTO + current → update request
        var updateRequest = dto.ToUpdateRequest(current, requestedByUserId);

        // 3) call Java update rpc
        var reply = await _client.UpdateCowAsync(updateRequest);
        return reply.ToDto();
    }

    // Delete cow
    public async Task DeleteAsync(long id)
    {
        // 1) ensure cow exists
        var existing = await _client.GetCowByIdAsync(new CowIdRequest { Id = id });

        if (existing == null || existing.Id == 0)
            throw new NotFoundException($"Cow with id {id} not found.");

        // 2) call delete rpc
        await _client.DeleteCowAsync(new CowIdRequest { Id = id });
    }
}
