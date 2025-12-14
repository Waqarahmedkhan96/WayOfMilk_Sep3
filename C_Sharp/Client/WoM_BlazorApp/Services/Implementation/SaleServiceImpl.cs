using System.Text.Json;
using ApiContracts;
using WoM_BlazorApp.Services.Interfaces;

namespace WoM_BlazorApp.Services.Implementation;

public class SaleServiceImpl (HttpClient client) : ISaleService
{

    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true
    };


    public async Task<ICollection<SaleDto>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<SaleDto> GetByIdAsync(long id)
    {
        throw new NotImplementedException();
    }

    public async Task<SaleDto> CreateAsync(CreateSaleDto dto)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateAsync(long id, UpdateSaleDto dto)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(long id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<SaleDto>> GetAllTrackedAsync()
    {
        var response = await client.GetAsync("sales");
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(await response.Content.ReadAsStringAsync());
        }


        var wrapper = await response.Content.ReadFromJsonAsync<SaleListDto>(_options);


        return wrapper?.Sales ?? new List<SaleDto>();
    }

    public async Task<SaleDto> GetTrackedByIdAsync(long id)
    {
        var response = await client.GetAsync($"sales/{id}");
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(await response.Content.ReadAsStringAsync());
        }
        return await response.Content.ReadFromJsonAsync<SaleDto>(_options)
               ?? throw new Exception("Failed to deserialize cow");
    }

}