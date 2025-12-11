using System.Net.Http.Json;
using System.Text.Json;
using ApiContracts.TransferRecords;

namespace WoM_BlazorApp.Http;

public class HttpTransferRecordService : ITransferRecordService
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public HttpTransferRecordService(HttpClient client)
    {
        _client = client;
    }

    public async Task<ICollection<TransferRecordDto>> GetAllAsync()
    {
        HttpResponseMessage response = await _client.GetAsync("transferrecords");
        string content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
            throw new Exception(content);

        var transferRecords = JsonSerializer.Deserialize<List<TransferRecordDto>>(content, _jsonOptions)!;
        return transferRecords;
    }

    public async Task<TransferRecordDto> GetByIdAsync(int id)
    {
        HttpResponseMessage response = await _client.GetAsync($"transferrecords/{id}");
        string content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
            throw new Exception(content);

        return JsonSerializer.Deserialize<TransferRecordDto>(content, _jsonOptions)!;
    }

    public async Task<TransferRecordDto> CreateAsync(CreateTransferRecordDto dto)
    {
        HttpResponseMessage httpResponse = await _client.PostAsJsonAsync("transferrecords", dto);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
            throw new Exception(response);

        return JsonSerializer.Deserialize<TransferRecordDto>(response, _jsonOptions)!;
    }

    public async Task UpdateAsync(int id, UpdateTransferRecordDto dto)
    {
        HttpResponseMessage httpResponse = await _client.PutAsJsonAsync($"transferrecords/{id}", dto);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
            throw new Exception(response);
    }

    public async Task DeleteAsync(int id)
    {
        HttpResponseMessage httpResponse = await _client.DeleteAsync($"transferrecords/{id}");
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
            throw new Exception(response);
    }
}
