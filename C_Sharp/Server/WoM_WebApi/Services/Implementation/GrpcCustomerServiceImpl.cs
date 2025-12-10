using ApiContracts;
using Sep3.WayOfMilk.Grpc;
using WoM_WebApi.GlobalExceptionHandler;
using WoM_WebApi.Mapping;
using WoM_WebApi.Services.Interfaces;

namespace WoM_WebApi.Services.Implementation;

// Impl: gRPC-based customer service
public class GrpcCustomerServiceImpl : ICustomerService
{
    private readonly CustomerService.CustomerServiceClient _client;

    public GrpcCustomerServiceImpl(CustomerService.CustomerServiceClient client)
    {
        _client = client;
    }

    // Create customer
    public async Task<CustomerDto> CreateAsync(CreateCustomerDto dto)
    {
        // 1) map DTO → gRPC
        var request = dto.ToGrpc();

        // 2) call Java create
        var reply = await _client.CreateCustomerAsync(request);

        if (reply == null || reply.Id == 0)
            throw new ValidationException("Unable to create customer.");

        return reply.ToDto();
    }

    // Get by id
    public async Task<CustomerDto> GetByIdAsync(long id)
    {
        // 1) call Java
        var reply = await _client.GetCustomerByIdAsync(new CustomerIdRequest { Id = id });

        // 2) validate
        if (reply == null || reply.Id == 0)
            throw new NotFoundException($"Customer with id {id} not found.");

        return reply.ToDto();
    }

    // Get all
    public async Task<CustomerListDto> GetAllAsync()
    {
        // 1) call Java list
        var reply = await _client.GetAllCustomersAsync(new Empty());

        // 2) map list
        return reply.ToListDto();
    }

    // Get by CVR
    public async Task<CustomerDto?> GetByCvrAsync(string cvr)
    {
        // 1) call Java
        var reply = await _client.GetCustomerByCVRAsync(new SentString { Value = cvr });

        // 2) if not found, return null
        return reply == null || reply.Id == 0 ? null : reply.ToDto();
    }

    // Get by name
    public async Task<CustomerListDto> GetByNameAsync(string namePart)
    {
        // 1) call Java search
        var reply = await _client.GetCustomerByNameAsync(new SentString { Value = namePart });

        // 2) map list
        return reply.ToListDto();
    }

    // Delete
    public async Task DeleteAsync(long id)
    {
        // 1) ensure exists
        var existing = await _client.GetCustomerByIdAsync(new CustomerIdRequest { Id = id });
        if (existing == null || existing.Id == 0)
            throw new NotFoundException($"Customer with id {id} not found.");

        // 2) call delete
        await _client.DeleteCustomerAsync(new SentId { Id = id });
    }
}
