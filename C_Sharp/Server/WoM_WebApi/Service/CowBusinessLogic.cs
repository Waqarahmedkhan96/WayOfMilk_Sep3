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

    public async Task<CowDto> UpdateCowAsync(CowDto dto, long requesterUserId)
    {
        // 1. Map DTO -> Proto Request
        var request = CowMapping.CowUpdateRequestToGrpc(dto, requesterUserId);

        // 2. Call the gRPC method
        CowData response = await _client.UpdateCowAsync(request);

        // 3. Map Proto Response -> DTO
        return CowMapping.CowGrpcToDto(response);
    }

    public async IAsyncEnumerable<CowDto> UpdateBatchAsync(IEnumerable<CowDto> dtos, long requesterUserId)
    {
        var requests = dtos.Select(dto => CowMapping.CowUpdateRequestToGrpc(dto, requesterUserId));
        foreach (var request in requests)
        {
            CowData response = await _client.UpdateCowAsync(request);
            yield return CowMapping.CowGrpcToDto(response);
        }
    }

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