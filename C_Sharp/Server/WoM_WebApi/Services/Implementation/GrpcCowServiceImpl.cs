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

    // --- CREATE ---
    public async Task<CowDto> CreateAsync(CowCreationDto dto)
    {
        var request = CowGrpcMapper.CreationDtoToCow(dto);

        // T3 defaults 'isHealthy' to false
        CowData response = await _client.AddCowAsync(request);

        // Validation: Ensure T3 actually created it
        if (response == null || response.Id == 0)
        {
            throw new Exception("Failed to create cow. Backend returned empty ID.");
            // If you have his 'ValidationException', throw that instead.
        }

        return CowGrpcMapper.ToDto(response);
    }

    // --- READ ---
    public async Task<CowDto> GetByIdAsync(long id)
    {
        var response = await _client.GetCowByIdAsync(new CowIdRequest { Id = id });

        // Validation: Ensure it exists
        if (response == null || response.Id == 0)
        {
            // Throw his 'NotFoundException' here if available
            throw new KeyNotFoundException($"Cow with id {id} not found.");
        }

        return CowGrpcMapper.ToDto(response);
    }

    public async Task<IEnumerable<CowDto>> GetAllAsync()
    {
        var response = await _client.GetAllCowsAsync(new Empty());
        return response.Cows.Select(CowGrpcMapper.ToDto);
    }

    public async Task<CowDto> GetByRegNoAsync(string regNo)
    {
        var response = await _client.GetCowByRegNoAsync(new SentString { Value = regNo });

        if (response == null || response.Id == 0)
        {
            throw new KeyNotFoundException($"Cow with RegNo {regNo} not found.");
        }

        return CowGrpcMapper.ToDto(response);
    }

    // --- UPDATE (General) ---
    public async Task<CowDto> UpdateCowAsync(CowDto dto, long requesterUserId)
    {
        // 1. Fetch Existing (Source of Truth)
        var existingCowProto = await _client.GetCowByIdAsync(new CowIdRequest { Id = dto.Id });

        if (existingCowProto == null || existingCowProto.Id == 0)
        {
            throw new KeyNotFoundException($"Cow with id {dto.Id} not found, cannot update.");
        }

        // 2. Prepare Request (Start with DB data)
        var request = new CowUpdateRequest
        {
            CowData = existingCowProto,
            RequestedBy = requesterUserId
        };

        // 3. Patch only allowed fields
        request.CowData.RegNo = dto.RegNo;
        // Handle potential null/missing BirthDate in DTO if necessary,
        // strictly speaking DTO has DateOnly (struct), so it's never null, just MinValue
        if (dto.BirthDate != DateOnly.MinValue)
        {
            request.CowData.BirthDate = dto.BirthDate.ToString("yyyy-MM-dd");
        }

        if (dto.DepartmentId.HasValue)
        {
            request.CowData.DepartmentId = dto.DepartmentId.Value;
        }

        // CRITICAL: Preserve Health Status (Do not touch IsHealthy)

        // 4. Send Update
        CowData response = await _client.UpdateCowAsync(request);
        return CowGrpcMapper.ToDto(response);
    }

    // --- UPDATE (Batch) ---
    public async IAsyncEnumerable<CowDto> UpdateBatchAsync(IEnumerable<CowDto> dtos, long requesterUserId)
    {
        foreach (var dto in dtos)
        {
            // Reuse the robust single update logic
            // Note: If one fails (NotFound), this loop will crash.
            // You might want to try-catch inside here if you want partial success.
            var result = await UpdateCowAsync(dto, requesterUserId);
            yield return result;
        }
    }

    // --- UPDATE (Health) ---
    public async IAsyncEnumerable<CowDto> UpdateCowsHealthAsync(
        IEnumerable<long> cowIds, bool healthUpdate, long requesterUserId)
    {
        var request = new CowsHealthChangeRequest();
        request.CowIds.AddRange(cowIds);
        request.NewHealthStatus = healthUpdate;
        request.RequestedByUserId = requesterUserId;

        var response = await _client.UpdateCowsHealthAsync(request);

        foreach (var item in response.Cows)
        {
            yield return CowGrpcMapper.ToDto(item);
        }
    }

    // --- DELETE ---
    public async Task DeleteAsync(long id)
    {
        // We skip the "GetBeforeDelete" check for efficiency unless strictly required.
        // If the ID doesn't exist, T3 usually ignores it or throws RpcException.
        await _client.DeleteCowAsync(new CowIdRequest { Id = id });
    }

    public async Task DeleteBatchAsync(long[] ids)
    {
        var failedIds = new List<long>();

        foreach (var id in ids)
        {
            try
            {
                await _client.DeleteCowAsync(new CowIdRequest { Id = id });
            }
            catch (Exception e)
            {
                // Simple logging for batch failures
                Console.WriteLine($"Failed to delete cow {id}: {e.Message}");
                failedIds.Add(id);
            }
        }

        if (failedIds.Count > 0)
        {
             Console.WriteLine($"Failed to delete {failedIds.Count} cows.");
        }
    }
}
