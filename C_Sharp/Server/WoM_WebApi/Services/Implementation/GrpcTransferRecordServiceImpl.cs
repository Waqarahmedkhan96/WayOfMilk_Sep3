using ApiContracts;
using Sep3.WayOfMilk.Grpc;
using WoM_WebApi.GlobalExceptionHandler;
using WoM_WebApi.Mapping;
using WoM_WebApi.Services.Interfaces;

namespace WoM_WebApi.Services.Implementation;

// Impl: gRPC-based transfer record service
public class GrpcTransferRecordServiceImpl : ITransferRecordService
{
    private readonly TransferRecordService.TransferRecordServiceClient _client;

    public GrpcTransferRecordServiceImpl(TransferRecordService.TransferRecordServiceClient client)
    {
        _client = client;
    }

    // Create transfer record
    public async Task<TransferRecordDto> CreateAsync(CreateTransferRecordDto dto)
    {
        // 1) map DTO → gRPC
        var request = dto.ToGrpc();

        // 2) call Java create
        var reply = await _client.AddTransferRecordAsync(request);

        if (reply == null || reply.Id == 0)
            throw new ValidationException("Unable to create transfer record.");

        return reply.ToDto();
    }

    // Get by id
    public async Task<TransferRecordDto> GetByIdAsync(long id)
    {
        // 1) call Java
        var reply = await _client.GetTransferRecordByIdAsync(
            new TransferRecordIdRequest { Id = id });

        // 2) validate
        if (reply == null || reply.Id == 0)
            throw new NotFoundException($"Transfer record with id {id} not found.");

        return reply.ToDto();
    }

    // Get all
    public async Task<TransferRecordListDto> GetAllAsync()
    {
        // 1) call Java list
        var reply = await _client.GetAllTransferRecordsAsync(new Empty());

        // 2) map list
        return reply.ToListDto();
    }

    // Get for cow
    public async Task<TransferRecordListDto> GetForCowAsync(long cowId)
    {
        // 1) call Java filter
        var reply = await _client.GetTransferRecordsForCowAsync(
            new TransferRecordsForCowRequest { CowId = cowId });

        // 2) map list
        return reply.ToListDto();
    }

    // Update existing record
    public async Task<TransferRecordDto> UpdateAsync(UpdateTransferRecordDto dto)
    {
        // 1) load current record
        var current = await _client.GetTransferRecordByIdAsync(
            new TransferRecordIdRequest { Id = dto.Id });

        if (current == null || current.Id == 0)
            throw new NotFoundException($"Transfer record with id {dto.Id} not found.");

        // 2) map dto + current → new record
        var updated = dto.ToUpdateRecord(current);

        // 3) call Java update
        var reply = await _client.UpdateTransferRecordAsync(updated);
        return reply.ToDto();
    }

    // Vet approves transfer
    public async Task ApproveAsync(ApproveTransferDto dto)
    {
        // 1) map DTO → gRPC request
        var request = dto.ToGrpc();

        // 2) call Java approve
        var reply = await _client.ApproveTransferAsync(request);

        if (reply == null || reply.Id == 0)
            throw new ValidationException("Unable to approve transfer record.");
    }

    // Delete
    public async Task DeleteAsync(long id)
    {
        // 1) ensure exists
        var existing = await _client.GetTransferRecordByIdAsync(
            new TransferRecordIdRequest { Id = id });

        if (existing == null || existing.Id == 0)
            throw new NotFoundException($"Transfer record with id {id} not found.");

        // 2) call delete
        await _client.DeleteTransferRecordAsync(new TransferRecordIdRequest { Id = id });
    }
}
