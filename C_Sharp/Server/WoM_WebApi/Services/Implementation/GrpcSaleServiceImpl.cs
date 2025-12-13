using ApiContracts;
using Sep3.WayOfMilk.Grpc;
using WoM_WebApi.GlobalExceptionHandler;
using WoM_WebApi.Mapping;
using WoM_WebApi.Services.Interfaces;

namespace WoM_WebApi.Services.Implementation;

// Impl: gRPC-based sale service
public class GrpcSaleServiceImpl : ISaleService
{
    private readonly SaleService.SaleServiceClient _client;

    public GrpcSaleServiceImpl(SaleService.SaleServiceClient client)
    {
        _client = client;
    }

    // Create sale
    public async Task<SaleDto> CreateAsync(CreateSaleDto dto)
    {
        // 1) map DTO → gRPC
        var request = dto.ToGrpc();

        // 2) call Java create
        var reply = await _client.CreateSaleAsync(request);

        if (reply == null || reply.Id == 0)
            throw new ValidationException("Unable to create sale.");

        return reply.ToDto();
    }

    // Get by id
    public async Task<SaleDto> GetByIdAsync(long id)
    {
        // 1) call Java
        var reply = await _client.GetSaleByIdAsync(new SaleIdRequest { Id = id });

        // 2) validate
        if (reply == null || reply.Id == 0)
            throw new NotFoundException($"Sale with id {id} not found.");

        return reply.ToDto();
    }

    // Get all
    public async Task<SaleListDto> GetAllAsync()
    {
        // 1) call Java list
        var reply = await _client.GetAllSalesAsync(new Empty());

        // 2) map list
        return reply.ToListDto();
    }

    // Delete
    public async Task DeleteAsync(long id)
    {
        // 1) ensure exists
        var existing = await _client.GetSaleByIdAsync(new SaleIdRequest { Id = id });
        if (existing == null || existing.Id == 0)
            throw new NotFoundException($"Sale with id {id} not found.");

        // 2) call delete
        await _client.DeleteSaleAsync(new SentId { Id = id });
    }
}
