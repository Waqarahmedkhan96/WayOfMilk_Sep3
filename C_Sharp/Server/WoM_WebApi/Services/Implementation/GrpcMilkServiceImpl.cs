using ApiContracts;
using Sep3.WayOfMilk.Grpc;
using WoM_WebApi.GlobalExceptionHandler;
using WoM_WebApi.Mapping;
using WoM_WebApi.Services.Interfaces;

namespace WoM_WebApi.Services.Implementation;

// Impl: gRPC-based milk service
public class GrpcMilkServiceImpl : IMilkService
{
    private readonly MilkService.MilkServiceClient _client;

    public GrpcMilkServiceImpl(MilkService.MilkServiceClient client)
    {
        _client = client;
    }

    // Create milk record
    public async Task<MilkDto> CreateAsync(CreateMilkDto dto)
    {
        // 1) map DTO → gRPC request
        var request = dto.ToGrpc();

        // 2) call Java create
        var reply = await _client.CreateMilkAsync(request);

        if (reply == null || reply.Id == 0)
            throw new ValidationException("Unable to create milk record.");

        return reply.ToDto();
    }

    // Get milk by id
    public async Task<MilkDto> GetByIdAsync(long id)
    {
        // 1) call Java get
        var reply = await _client.GetMilkAsync(new SentId { Id = id });

        // 2) validate existence
        if (reply == null || reply.Id == 0)
            throw new NotFoundException($"Milk record with id {id} not found.");

        return reply.ToDto();
    }

    // Get all milk records
    public async Task<MilkListDto> GetAllAsync()
    {
        // 1) call Java list
        var reply = await _client.GetAllMilkAsync(new Empty());

        // 2) map list
        return reply.ToListDto();
    }

    // Get by container
    public async Task<MilkListDto> GetByContainerAsync(long containerId)
    {
        // 1) call Java filter
        var reply = await _client.GetMilkByContainerAsync(
            new MilkByContainerQuery { ContainerId = containerId });

        // 2) map list
        return reply.ToListDto();
    }

    // Update record
    public async Task<MilkDto> UpdateAsync(UpdateMilkDto dto)
    {
        // 1) load current record
        var current = await _client.GetMilkAsync(new SentId { Id = dto.Id });
        if (current == null || current.Id == 0)
            throw new NotFoundException($"Milk record with id {dto.Id} not found.");

        // 2) map dto + current → update request
        var request = dto.ToUpdateRequest(current);

        // 3) call Java update
        var reply = await _client.UpdateMilkAsync(request);
        return reply.ToDto();
    }

    // Approve / deny storage
    public async Task ApproveStorageAsync(ApproveMilkStorageDto dto)
    {
        // 1) map DTO → gRPC request
        var request = dto.ToGrpc();

        // 2) call Java approve
        var reply = await _client.ApproveMilkStorageAsync(request);

        if (reply == null || reply.Id == 0)
            throw new ValidationException("Unable to approve milk storage.");
    }

    // Delete
    public async Task DeleteAsync(long id)
    {
        // 1) ensure exists
        var current = await _client.GetMilkAsync(new SentId { Id = id });
        if (current == null || current.Id == 0)
            throw new NotFoundException($"Milk record with id {id} not found.");

        // 2) call delete
        await _client.DeleteMilkAsync(new SentId { Id = id });
    }
}
