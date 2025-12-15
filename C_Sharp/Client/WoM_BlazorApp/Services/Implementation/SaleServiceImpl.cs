using System.Net.Http.Json;
using System.Text.Json;
using ApiContracts;
using WoM_BlazorApp.Services.Interfaces;

namespace WoM_BlazorApp.Services.Implementation;

public class SaleServiceImpl(HttpClient client) : ISaleService
{
    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true
    };

    // MAIN METHODS USED BY YOUR PAGES
    public async Task<ICollection<SaleDto>> GetAllAsync()
    {
        var response = await client.GetAsync("sales");
        if (!response.IsSuccessStatusCode)
            throw new Exception(await response.Content.ReadAsStringAsync());

        // API returns a wrapper: SaleListDto { List<SaleDto> Sales }
        var wrapper = await response.Content.ReadFromJsonAsync<SaleListDto>(_options);
        return wrapper?.Sales ?? new List<SaleDto>();
    }

    public async Task<SaleDto> GetByIdAsync(long id)
    {
        var response = await client.GetAsync($"sales/{id}");
        if (!response.IsSuccessStatusCode)
            throw new Exception(await response.Content.ReadAsStringAsync());

        return await response.Content.ReadFromJsonAsync<SaleDto>(_options)
               ?? throw new Exception("Failed to deserialize sale");
    }

    public async Task<SaleDto> CreateAsync(CreateSaleDto dto)
    {
        var response = await client.PostAsJsonAsync("sales", dto);
        if (!response.IsSuccessStatusCode)
            throw new Exception(await response.Content.ReadAsStringAsync());

        return await response.Content.ReadFromJsonAsync<SaleDto>(_options)
               ?? throw new Exception("Failed to deserialize sale");
    }

    public async Task UpdateAsync(long id, UpdateSaleDto dto)
    {
        var response = await client.PutAsJsonAsync($"sales/{id}", dto);
        if (!response.IsSuccessStatusCode)
            throw new Exception(await response.Content.ReadAsStringAsync());
    }

    public async Task DeleteAsync(long id)
    {
        var response = await client.DeleteAsync($"sales/{id}");
        if (!response.IsSuccessStatusCode)
            throw new Exception(await response.Content.ReadAsStringAsync());
    }

    // MOCKING / TRACKING METHODS (you already used these in Traceability)
    public async Task<IEnumerable<SaleDto>> GetAllTrackedAsync()
    {
        var response = await client.GetAsync("sales");
        if (!response.IsSuccessStatusCode)
            throw new Exception(await response.Content.ReadAsStringAsync());

        var wrapper = await response.Content.ReadFromJsonAsync<SaleListDto>(_options);
        return wrapper?.Sales ?? new List<SaleDto>();
    }

    public async Task<SaleDto> GetTrackedByIdAsync(long id)
    {
        var response = await client.GetAsync($"sales/{id}");
        if (!response.IsSuccessStatusCode)
            throw new Exception(await response.Content.ReadAsStringAsync());

        return await response.Content.ReadFromJsonAsync<SaleDto>(_options)
               ?? throw new Exception("Failed to deserialize sale");
    }
}
