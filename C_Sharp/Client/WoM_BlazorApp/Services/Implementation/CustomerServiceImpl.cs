using System.Text.Json;
using ApiContracts;
using WoM_BlazorApp.Services.Interfaces;

namespace WoM_BlazorApp.Services.Implementation;

public class CustomerServiceImpl(HttpClient client) : ICustomerService
{
    //token relevant
    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true
    };
    public async Task<ICollection<CustomerDto>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<CustomerDto> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<CustomerDto> CreateAsync(CreateCustomerDto dto)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateAsync(int id, UpdateCustomerDto dto)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(long id)
    {
        throw new NotImplementedException();
    }

    //mocking relevant

    public async Task<IEnumerable<CustomerDto>> GetAllTrackedAsync()
    {
        var response = await client.GetAsync("customers");
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(await response.Content.ReadAsStringAsync());
        }

        // FIX: Deserialize to the Wrapper DTO first, not the list directly.
        var wrapper = await response.Content.ReadFromJsonAsync<CustomerListDto>(_options);

        // Access the list property inside.
        // CHECK THIS: It might be .Customers, .Items, or .List depending on your ApiContracts.
        return wrapper?.Customers ?? new List<CustomerDto>();
    }
}