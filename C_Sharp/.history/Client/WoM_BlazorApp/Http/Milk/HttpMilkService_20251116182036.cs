using System.Net.Http.Json;
using System.Text.Json;
using ApiContracts.Cows;

namespace WoM_BlazorApp.Http;

public class HttpCowService : ICowService
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public HttpCowService(HttpClient client)
    {
        _client = client;
    }

    public async Task<ICollection<CowDto>> GetAllAsync()
    {
        HttpResponseMessage response = await _client.GetAsync("cows");
        string content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
            throw new Exception(content);

        var cows = JsonSerializer.Deserialize<List<CowDto>>(content, _jsonOptions)!;
        return cows;
    }

    public async Task<CowDto> GetByIdAsync(int id)
    {
        HttpResponseMessage response = await _client.GetAsync($"cows/{id}");
        string content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
            throw new Exception(content);

        return JsonSerializer.Deserialize<CowDto>(content, _jsonOptions)!;
    }

    public async Task<CowDto> CreateAsync(CreateCowDto dto)
    {
        HttpResponseMessage httpResponse = await _client.PostAsJsonAsync("cows", dto);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
            throw new Exception(response);

        return JsonSerializer.Deserialize<CowDto>(response, _jsonOptions)!;
    }

    public async Task UpdateAsync(int id, UpdateCowDto dto)
    {
        HttpResponseMessage httpResponse = await _client.PutAsJsonAsync($"cows/{id}", dto);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
            throw new Exception(response);
    }

    public async Task DeleteAsync(int id)
    {
        HttpResponseMessage httpResponse = await _client.DeleteAsync($"cows/{id}");
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
            throw new Exception(response);
    }
}
