using System.Net.Http.Json;
using System.Text.Json;
using ApiContracts.Customers;

namespace WoM_BlazorApp.Http;

public class HttpCustomerService : ICustomerService
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public HttpCustomerService(HttpClient client)
    {
        _client = client;
    }

    public async Task<ICollection<CustomerDto>> GetAllAsync()
    {
        HttpResponseMessage response = await _client.GetAsync("customers");
        string content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
            throw new Exception(content);

        var customers = JsonSerializer.Deserialize<List<CustomerDto>>(content, _jsonOptions)!;
        return customers;
    }

    public async Task<CustomerDto> GetByIdAsync(int id)
    {
        HttpResponseMessage response = await _client.GetAsync($"customers/{id}");
        string content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
            throw new Exception(content);

        return JsonSerializer.Deserialize<CustomerDto>(content, _jsonOptions)!;
    }

    public async Task<CustomerDto> CreateAsync(CreateCustomerDto dto)
    {
        HttpResponseMessage httpResponse = await _client.PostAsJsonAsync("customers", dto);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
            throw new Exception(response);

        return JsonSerializer.Deserialize<CustomerDto>(response, _jsonOptions)!;
    }

    public async Task UpdateAsync(int id, UpdateCustomerDto dto)
    {
        HttpResponseMessage httpResponse = await _client.PutAsJsonAsync($"customers/{id}", dto);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
            throw new Exception(response);
    }

    public async Task DeleteAsync(int id)
    {
        HttpResponseMessage httpResponse = await _client.DeleteAsync($"customers/{id}");
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
            throw new Exception(response);
    }
}
