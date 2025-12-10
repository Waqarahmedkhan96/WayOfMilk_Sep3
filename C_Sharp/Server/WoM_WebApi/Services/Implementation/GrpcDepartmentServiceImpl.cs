using ApiContracts;
using Sep3.WayOfMilk.Grpc;
using WoM_WebApi.GlobalExceptionHandler;
using WoM_WebApi.Mapping;
using WoM_WebApi.Services.Interfaces;

namespace WoM_WebApi.Services.Implementation;

// Impl: gRPC-based department service
public class GrpcDepartmentServiceImpl : IDepartmentService
{
    private readonly DepartmentService.DepartmentServiceClient _client;

    public GrpcDepartmentServiceImpl(DepartmentService.DepartmentServiceClient client)
    {
        _client = client;
    }

    // Create department
    public async Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto)
    {
        // 1) map DTO → gRPC
        var request = dto.ToGrpc();

        // 2) call Java create
        var reply = await _client.AddDepartmentAsync(request);

        if (reply == null || reply.Id == 0)
            throw new ValidationException("Unable to create department.");

        return reply.ToDto();
    }

    // Get by id
    public async Task<DepartmentDto> GetByIdAsync(long id)
    {
        // 1) call Java
        var reply = await _client.GetDepartmentByIdAsync(new DepartmentIdRequest { Id = id });

        // 2) validate
        if (reply == null || reply.Id == 0)
            throw new NotFoundException($"Department with id {id} not found.");

        return reply.ToDto();
    }

    // Get all
    public async Task<DepartmentListDto> GetAllAsync()
    {
        // 1) call Java list
        var reply = await _client.GetAllDepartmentsAsync(new Empty());

        // 2) map list
        return reply.ToListDto();
    }

    // Get by type
    public async Task<DepartmentListDto> GetByTypeAsync(DepartmentType type)
    {
        // 1) call Java by type
        var reply = await _client.GetDepartmentsByTypeAsync(
            new DepartmentTypeRequest { Type = type.ToString().ToUpperInvariant() });

        // 2) map list
        return reply.ToListDto();
    }

    // Update
    public async Task<DepartmentDto> UpdateAsync(long id, UpdateDepartmentDto dto)
    {
        // 1) ensure exists
        var existing = await _client.GetDepartmentByIdAsync(new DepartmentIdRequest { Id = id });
        if (existing == null || existing.Id == 0)
            throw new NotFoundException($"Department with id {id} not found.");

        // 2) map DTO → gRPC
        var request = dto.ToGrpc(id);

        // 3) call Java update
        var reply = await _client.UpdateDepartmentAsync(request);
        return reply.ToDto();
    }

    // Delete
    public async Task DeleteAsync(long id)
    {
        // 1) ensure exists
        var existing = await _client.GetDepartmentByIdAsync(new DepartmentIdRequest { Id = id });
        if (existing == null || existing.Id == 0)
            throw new NotFoundException($"Department with id {id} not found.");

        // 2) call delete
        await _client.DeleteDepartmentAsync(new DepartmentIdRequest { Id = id });
    }
}
