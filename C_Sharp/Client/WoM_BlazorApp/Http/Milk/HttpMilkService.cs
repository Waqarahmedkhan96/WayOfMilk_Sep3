using System.Net.Http.Json;
using System.Text.Json;
using ApiContracts.Milk;

namespace WoM_BlazorApp.Http;

public class HttpMilkService : IMilkService
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public HttpMilkService(HttpClient client)
    {
        _client = client;
    }

    public async Task<ICollection<MilkDto>> GetAllAsync()
    {
        HttpResponseMessage response = await _client.GetAsync("milk");
        string content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
            throw new Exception(content);

        var milks = JsonSerializer.Deserialize<List<MilkDto>>(content, _jsonOptions)!;
        return milks;
    }

    public async Task<MilkDto> GetByIdAsync(int id)
    {
        HttpResponseMessage response = await _client.GetAsync($"milk/{id}");
        string content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
            throw new Exception(content);

        return JsonSerializer.Deserialize<MilkDto>(content, _jsonOptions)!;
    }

    public async Task<MilkDto> CreateAsync(CreateMilkDto dto)
    {
        HttpResponseMessage httpResponse = await _client.PostAsJsonAsync("milk", dto);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
            throw new Exception(response);

        return JsonSerializer.Deserialize<MilkDto>(response, _jsonOptions)!;
    }

    public async Task UpdateAsync(int id, UpdateMilkDto dto)
    {
        HttpResponseMessage httpResponse = await _client.PutAsJsonAsync($"milk/{id}", dto);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
            throw new Exception(response);
    }

    public async Task DeleteAsync(int id)
    {
        HttpResponseMessage httpResponse = await _client.DeleteAsync($"milk/{id}");
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
            throw new Exception(response);
    }
}
