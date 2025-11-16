using System.Net.Http.Json;
using System.Text.Json;
using ApiContracts.Containers;

namespace WoM_BlazorApp.Http;

public class HttpContainerService : IContainerService
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public HttpContainerService(HttpClient client)
    {
        _client = client;
    }

    public async Task<ICollection<ContainerDto>> GetAllAsync()
    {
        HttpResponseMessage response = await _client.GetAsync("containers");
        string content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
            throw new Exception(content);

        var containers = JsonSerializer.Deserialize<List<ContainerDto>>(content, _jsonOptions)!;
        return containers;
    }

    public async Task<ContainerDto> GetByIdAsync(int id)
    {
        HttpResponseMessage response = await _client.GetAsync($"containers/{id}");
        string content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
            throw new Exception(content);

        return JsonSerializer.Deserialize<ContainerDto>(content, _jsonOptions)!;
    }

    public async Task<ContainerDto> CreateAsync(CreateContainerDto dto)
    {
        HttpResponseMessage httpResponse = await _client.PostAsJsonAsync("containers", dto);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
            throw new Exception(response);

        return JsonSerializer.Deserialize<ContainerDto>(response, _jsonOptions)!;
    }

    public async Task UpdateAsync(int id, UpdateContainerDto dto)
    {
        HttpResponseMessage httpResponse = await _client.PutAsJsonAsync($"containers/{id}", dto);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
            throw new Exception(response);
    }

    public async Task DeleteAsync(int id)
    {
        HttpResponseMessage httpResponse = await _client.DeleteAsync($"containers/{id}");
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
            throw new Exception(response);
    }
}
