using System.Net.Http.Json;
using System.Text.Json;
using ApiContracts.Sales;

namespace WoM_BlazorApp.Http;

public class HttpSaleService : ISaleService
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public HttpSaleService(HttpClient client)
    {
        _client = client;
    }

    public async Task<ICollection<SaleDto>> GetAllAsync()
    {
        HttpResponseMessage response = await _client.GetAsync("sales");
        string content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
            throw new Exception(content);

        var sales = JsonSerializer.Deserialize<List<SaleDto>>(content, _jsonOptions)!;
        return sales;
    }

    public async Task<SaleDto> GetByIdAsync(int id)
    {
        HttpResponseMessage response = await _client.GetAsync($"sales/{id}");
        string content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
            throw new Exception(content);

        return JsonSerializer.Deserialize<SaleDto>(content, _jsonOptions)!;
    }

    public async Task<SaleDto> CreateAsync(CreateSaleDto dto)
    {
        HttpResponseMessage httpResponse = await _client.PostAsJsonAsync("sales", dto);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
            throw new Exception(response);

        return JsonSerializer.Deserialize<SaleDto>(response, _jsonOptions)!;
    }

    public async Task UpdateAsync(int id, UpdateSaleDto dto)
    {
        HttpResponseMessage httpResponse = await _client.PutAsJsonAsync($"sales/{id}", dto);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
            throw new Exception(response);
    }

    public async Task DeleteAsync(int id)
    {
        HttpResponseMessage httpResponse = await _client.DeleteAsync($"sales/{id}");
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
            throw new Exception(response);
    }
}
