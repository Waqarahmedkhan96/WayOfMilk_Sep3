using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using ApiContracts;
using WoM_BlazorApp.Services.Interfaces;

namespace WoM_BlazorApp.Services.Implementation;

public class TransferRecordServiceImpl : ITransferRecordService
{
    private readonly HttpClient _client;

    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    public TransferRecordServiceImpl(HttpClient client)
    {
        _client = client;
    }

    public async Task<ICollection<TransferRecordDto>> GetAllAsync()
    {
        var response = await _client.GetAsync("transfers");
        response.EnsureSuccessStatusCode();

        var wrapper = await response.Content
            .ReadFromJsonAsync<TransferRecordListDto>(_options);

        return wrapper?.Transfers ?? new List<TransferRecordDto>();

    }

    public async Task<TransferRecordDto> GetByIdAsync(int id)
    {
        return await _client.GetFromJsonAsync<TransferRecordDto>($"transfers/{id}", _options)
               ?? throw new Exception("Transfer not found");
    }

    public async Task<TransferRecordDto> CreateAsync(CreateTransferRecordDto dto)
    {
        var response = await _client.PostAsJsonAsync("transfers", dto, _options);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<TransferRecordDto>(_options)
               ?? throw new Exception("Failed to create transfer");
    }

    public async Task UpdateAsync(int id, UpdateTransferRecordDto dto)
    {
        var response = await _client.PutAsJsonAsync($"transfers/{id}", dto, _options);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(long id)
    {
        var response = await _client.DeleteAsync($"transfers/{id}");
        response.EnsureSuccessStatusCode();
    }
}
