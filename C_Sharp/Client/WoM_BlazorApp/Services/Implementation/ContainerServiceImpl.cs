using System.Net.Http.Json;
using System.Text.Json;
using ApiContracts;
using WoM_BlazorApp.Services.Interfaces;

namespace WoM_BlazorApp.Services.Implementation;

public class ContainerServiceImpl : IContainerService
{
    private readonly HttpClient _http;
    private readonly JsonSerializerOptions _json;

    public ContainerServiceImpl(HttpClient http)
    {
        _http = http;
        _json = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task<ContainerListDto> GetAllAsync()
    {
        var result = await _http.GetFromJsonAsync<ContainerListDto>("containers", _json);
        return result ?? new ContainerListDto();
    }

    public async Task<ContainerDto> GetByIdAsync(long id)
    {
        var result = await _http.GetFromJsonAsync<ContainerDto>($"containers/{id}", _json);
        return result ?? throw new Exception("Container not found.");
    }

    public async Task<ContainerDto> CreateAsync(CreateContainerDto dto)
    {
        var response = await _http.PostAsJsonAsync("containers", dto, _json);
        if (!response.IsSuccessStatusCode)
            throw new Exception(await response.Content.ReadAsStringAsync());

        var created = await response.Content.ReadFromJsonAsync<ContainerDto>(_json);
        return created ?? throw new Exception("Create container returned empty response.");
    }

    public async Task<ContainerDto> UpdateAsync(long id, UpdateContainerDto dto)
    {
        dto.Id = id;

        var response = await _http.PutAsJsonAsync($"containers/{id}", dto, _json);
        if (!response.IsSuccessStatusCode)
            throw new Exception(await response.Content.ReadAsStringAsync());

        var updated = await response.Content.ReadFromJsonAsync<ContainerDto>(_json);
        return updated ?? throw new Exception("Update container returned empty response.");
    }

    public async Task DeleteAsync(long id)
    {
        var response = await _http.DeleteAsync($"containers/{id}");
        if (!response.IsSuccessStatusCode)
            throw new Exception(await response.Content.ReadAsStringAsync());
    }
}
