using ApiContracts;
using Grpc.Net.Client;
using Sep3.WayOfMilk.Grpc;
using WoM_WebApi.Mapping;

namespace WoM_WebApi.Service;

public interface ICowBusinessLogic
{
    Task<CowDto> CreateAsync(CowCreationDto dto);
    Task<CowDto> GetByIdAsync(long id);
    Task<IEnumerable<CowDto>> GetAllAsync();
    Task<CowDto> GetByRegNoAsync(string regNo);
    Task<CowDto> UpdateCowAsync(CowDto dto, long requesterUserId);
    IAsyncEnumerable<CowDto> UpdateBatchAsync(IEnumerable<CowDto> dtos,
        long requesterUserId);
    IAsyncEnumerable<CowDto> UpdateCowsHealthAsync(
    IEnumerable<long> CowIds, bool healthUpdate,long requesterUserId);
    Task DeleteAsync(long id);
    Task DeleteBatchAsync(long[] ids);

}

public class CowBusinessLogic : ICowBusinessLogic
{
    // inject the generated gRPC client here
    private readonly CowService.CowServiceClient _client;

    public CowBusinessLogic(CowService.CowServiceClient client)
    {
        _client = client;
    }

    //CREATE

    public async Task<CowDto> CreateAsync(CowCreationDto dto)
    {
        // 1. Map DTO -> Proto Request
        var request = CowMapping.CowCreationDtoToCow(dto);

        // 2. Call the gRPC method
        // T3 defaults 'isHealthy' to false internally
        CowData response = await _client.AddCowAsync(request);

        // 3. Map Proto Response -> DTO
        return CowMapping.CowGrpcToDto(response);
    }

    //READ/GET

    public async Task<CowDto> GetByIdAsync(long id)
    {
        // gRPC uses an Empty message for void arguments
        var response = await _client.GetCowByIdAsync(new CowIdRequest { Id = id });
        return CowMapping.CowGrpcToDto(response);
    }

    public async Task<IEnumerable<CowDto>> GetAllAsync()
    {
        // gRPC uses an Empty message for void arguments
        var response = await _client.GetAllCowsAsync(new Empty());

        // Convert the repeated list to C# IEnumerable
        return response.Cows.Select(CowMapping.CowGrpcToDto);
    }

    public async Task<CowDto> GetByRegNoAsync(string regNo)
    {
        SentString sent = new SentString { Value = regNo };
        var response = await _client.GetCowByRegNoAsync(sent);

        return CowMapping.CowGrpcToDto(response);

    }

    //UPDATE

    //Only Owner can change OTHER cow data fields
    // This implies this method should NEVER change the Health status, since the vet doesn't have access to it'
    public async Task<CowDto> UpdateCowAsync(CowDto dto, long requesterUserId)
    {
        // Fetch the EXISTING cow first
        // We do this to get the current reliable 'IsHealthy' status from the database.
        // (We cannot trust the 'Healthy' bool coming from the frontend DTO in a general update)
        var existingCowProto = await _client.GetCowByIdAsync(new CowIdRequest { Id = dto.Id });

        // Map DTO -> Request
        var request = CowMapping.CowUpdateRequestToGrpc(dto, requesterUserId);

        // OVERRIDE the health in the request with the EXISTING health.
        // This effectively makes the 'Healthy' field "Read-Only" for this endpoint.
        request.CowData.IsHealthy = existingCowProto.IsHealthy;

        // Send the update request
        CowData response = await _client.UpdateCowAsync(request);
        return CowMapping.CowGrpcToDto(response);
    }

    public async IAsyncEnumerable<CowDto> UpdateBatchAsync(IEnumerable<CowDto> dtos, long requesterUserId)
    {
        foreach (var dto in dtos)
        {
            // Re-use the logic above to ensure safety for every single item in the batch
            // (Yes, this is N+N calls, but it ensures safety without changing T3)

            // 1. Fetch Existing
            var existingCowProto = await _client.GetCowByIdAsync(new CowIdRequest { Id = dto.Id });

            // 2. Prepare Request & Override Health
            var request = CowMapping.CowUpdateRequestToGrpc(dto, requesterUserId);
            request.CowData.IsHealthy = existingCowProto.IsHealthy;

            // 3. Update
            CowData response = await _client.UpdateCowAsync(request);
            yield return CowMapping.CowGrpcToDto(response);
        }
    }

    //rule: only vet can change a health status to positive (healthy)
    //This is the ONLY method allowed to change health.
    //The Controller enforces the Role (Vet/Owner) access.
    public async IAsyncEnumerable<CowDto> UpdateCowsHealthAsync(
        IEnumerable<long> cowIds, bool healthUpdate,long requesterUserId)
    {
        var request = new CowsHealthChangeRequest();
        request.CowIds.AddRange(cowIds);
        request.NewHealthStatus = healthUpdate; // 'optional' in proto, but this sets it
        request.RequestedByUserId = requesterUserId;

        // Added 'await' and 'Async'. gRPC calls are asynchronous.
        // the return type is CowList, so we iterate response.Cows
        var response = await _client.UpdateCowsHealthAsync(request);

        foreach (var item in response.Cows)
        {
            yield return CowMapping.CowGrpcToDto(item);
        }
    }


    //DELETE

    public async Task DeleteAsync(long id)
    {
        await _client.DeleteCowAsync(new CowIdRequest { Id = id });
    }

    public async Task DeleteBatchAsync(long[] ids)
    {
        // 1. Iterate through the array of IDs
        var failedIds = new List<long>();

        foreach (var id in ids)
        {
            try
            {
                await _client.DeleteCowAsync(new CowIdRequest { Id = id });
            }
            catch (Exception e)
            {
                // Log the individual failure and keep going
                // In production, use ILogger instead of Console.WriteLine
                Console.WriteLine($"Failed to delete cow {id}: {e.Message}");
                failedIds.Add(id);
            }
        }
        if (failedIds.Count > 0)
        {
            string errorMessage = $"Failed to delete {failedIds.Count} cows: {string.Join(", ", failedIds.Select(id => id.ToString()).ToArray())}";
            //separated the message to avoid ambiguity error
            Console.WriteLine(errorMessage);
        }
    }


}