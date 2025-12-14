using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using ApiContracts;
using WoM_BlazorApp.Services.Interfaces;

namespace WoM_BlazorApp.Services.Implementation;

public class CowServiceImpl(HttpClient client) : ICowService
{
    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<IEnumerable<CowDto>> GetAllAsync()
    {
        var response = await client.GetAsync("cows");
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(await response.Content.ReadAsStringAsync());
        }
        return await response.Content.ReadFromJsonAsync<IEnumerable<CowDto>>(_options)
               ?? new List<CowDto>();
    }

    public async Task<CowDto> GetByIdAsync(long id)
    {
        var response = await client.GetAsync($"cows/{id}");
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(await response.Content.ReadAsStringAsync());
        }
        return await response.Content.ReadFromJsonAsync<CowDto>(_options)
               ?? throw new Exception("Failed to deserialize cow");
    }

    public async Task CreateAsync(CowCreationDto dto)
    {
        var response = await client.PostAsJsonAsync("cows/add", dto);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(await response.Content.ReadAsStringAsync());
        }
    }

    public async Task UpdateAsync(long id, CowDto dto)
    {
        var response = await client.PutAsJsonAsync($"cows/{id}", dto);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(await response.Content.ReadAsStringAsync());
        }
    }

    public async Task DeleteAsync(long id)
    {
        var response = await client.DeleteAsync($"cows/{id}");
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(await response.Content.ReadAsStringAsync());
        }
    }

    public async Task UpdateHealthAsync(IEnumerable<long> cowIds, bool newStatus)
    {
        var request = new UpdateHealthRequest
        {
            CowIds = cowIds,
            NewHealthStatus = newStatus
        };

        var response = await client.PutAsJsonAsync("cows/health", request);

        if (!response.IsSuccessStatusCode)
        {
            // Catches 403 Forbidden (Non-Vet making cow healthy)
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception(error);
        }
    }

    public async Task<IEnumerable<CowDto>> GetCowsByDepartmentAsync(long departmentId)
    {
        var response = await client.GetAsync($"cows/department/{departmentId}");

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(await response.Content.ReadAsStringAsync());
        }

        return await response.Content.ReadFromJsonAsync<IEnumerable<CowDto>>(_options)
               ?? new List<CowDto>();
    }

    public async Task<IEnumerable<MilkDto>> GetMilkByCowIdAsync(long cowId)
    {
        var response = await client.GetAsync($"cows/milk/{cowId}");

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(await response.Content.ReadAsStringAsync());
        }

        return await response.Content.ReadFromJsonAsync<IEnumerable<MilkDto>>(_options)
               ?? new List<MilkDto>();
    }
}