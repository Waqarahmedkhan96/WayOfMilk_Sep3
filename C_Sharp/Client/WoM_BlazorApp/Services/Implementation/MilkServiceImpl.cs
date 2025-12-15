using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using ApiContracts;
using WoM_BlazorApp.Services.Interfaces;

namespace WoM_BlazorApp.Services.Implementation;

public class MilkServiceImpl : IMilkService
{
    private readonly HttpClient _http;
    private readonly JsonSerializerOptions _json;

    public MilkServiceImpl(HttpClient http)
    {
        _http = http;

        _json = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() } // supports enum-as-string if needed
        };
    }

    public async Task<MilkListDto> GetAllAsync()
    {
        var result = await _http.GetFromJsonAsync<MilkListDto>("milk", _json);
        return result ?? new MilkListDto();
    }

    public async Task<MilkDto> GetByIdAsync(long id)
    {
        var result = await _http.GetFromJsonAsync<MilkDto>($"milk/{id}", _json);
        return result ?? throw new Exception("Milk record not found.");
    }

    public async Task<MilkListDto> GetByContainerAsync(long containerId)
    {
        var result = await _http.GetFromJsonAsync<MilkListDto>($"milk/by-container/{containerId}", _json);
        return result ?? new MilkListDto();
    }

    public async Task<MilkDto> CreateAsync(CreateMilkDto dto)
    {
        var response = await _http.PostAsJsonAsync("milk", dto, _json);
        if (!response.IsSuccessStatusCode)
            throw new Exception(await response.Content.ReadAsStringAsync());

        var created = await response.Content.ReadFromJsonAsync<MilkDto>(_json);
        return created ?? throw new Exception("Create milk returned empty response.");
    }

    public async Task<MilkDto> UpdateAsync(long id, UpdateMilkDto dto)
    {
        dto.Id = id;

        var response = await _http.PutAsJsonAsync($"milk/{id}", dto, _json);
        if (!response.IsSuccessStatusCode)
            throw new Exception(await response.Content.ReadAsStringAsync());

        var updated = await response.Content.ReadFromJsonAsync<MilkDto>(_json);
        return updated ?? throw new Exception("Update milk returned empty response.");
    }

    public async Task ApproveAsync(long id, ApproveMilkStorageDto dto)
    {
        dto.Id = id;

        var response = await _http.PostAsJsonAsync($"milk/{id}/approve", dto, _json);
        if (!response.IsSuccessStatusCode)
            throw new Exception(await response.Content.ReadAsStringAsync());
    }

    public async Task DeleteAsync(long id)
    {
        var response = await _http.DeleteAsync($"milk/{id}");
        if (!response.IsSuccessStatusCode)
            throw new Exception(await response.Content.ReadAsStringAsync());
    }
}
