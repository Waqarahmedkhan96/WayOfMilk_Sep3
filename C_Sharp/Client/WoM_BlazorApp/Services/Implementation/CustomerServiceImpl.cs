using System.Net.Http.Json;
using System.Text.Json;
using ApiContracts;
using WoM_BlazorApp.Services.Interfaces;

namespace WoM_BlazorApp.Services.Implementation;

public class CustomerServiceImpl(HttpClient client) : ICustomerService
{
    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true
    };

    // ---- NORMAL CRUD ----

    public async Task<ICollection<CustomerDto>> GetAllAsync()
    {
        var response = await client.GetAsync("customers");
        if (!response.IsSuccessStatusCode)
            throw new Exception(await response.Content.ReadAsStringAsync());

        var wrapper = await response.Content.ReadFromJsonAsync<CustomerListDto>(_options);
        return wrapper?.Customers ?? new List<CustomerDto>();
    }

    public async Task<CustomerDto> GetByIdAsync(int id)
    {
        var response = await client.GetAsync($"customers/{id}");
        if (!response.IsSuccessStatusCode)
            throw new Exception(await response.Content.ReadAsStringAsync());

        return await response.Content.ReadFromJsonAsync<CustomerDto>(_options)
               ?? throw new Exception("Failed to deserialize customer");
    }

    public async Task<CustomerDto> CreateAsync(CreateCustomerDto dto)
    {
        var response = await client.PostAsJsonAsync("customers", dto);
        if (!response.IsSuccessStatusCode)
            throw new Exception(await response.Content.ReadAsStringAsync());

        return await response.Content.ReadFromJsonAsync<CustomerDto>(_options)
               ?? throw new Exception("Failed to deserialize created customer");
    }

    public async Task UpdateAsync(int id, UpdateCustomerDto dto)
    {
        var response = await client.PutAsJsonAsync($"customers/{id}", dto);
        if (!response.IsSuccessStatusCode)
            throw new Exception(await response.Content.ReadAsStringAsync());
    }

    public async Task DeleteAsync(long id)
    {
        var response = await client.DeleteAsync($"customers/{id}");
        if (!response.IsSuccessStatusCode)
            throw new Exception(await response.Content.ReadAsStringAsync());
    }

    // ---- MOCK / TRACKING ----

    public async Task<IEnumerable<CustomerDto>> GetAllTrackedAsync()
    {
        var response = await client.GetAsync("customers");
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(await response.Content.ReadAsStringAsync());
        }

        var wrapper = await response.Content.ReadFromJsonAsync<CustomerListDto>(_options);
        return wrapper?.Customers ?? new List<CustomerDto>();
    }
}
