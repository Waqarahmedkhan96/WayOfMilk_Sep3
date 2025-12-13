using System.Net.Http.Json;
using System.Text.Json;
using ApiContracts;

namespace WoM_BlazorApp.Http;

public class HttpDepartmentService : IDepartmentService
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public HttpDepartmentService(HttpClient client)
    {
        _client = client;
    }

    public async Task<ICollection<DepartmentDto>> GetAllAsync()
    {
        HttpResponseMessage response = await _client.GetAsync("departments");
        string content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
            throw new Exception(content);

        return JsonSerializer.Deserialize<List<DepartmentDto>>(content, _jsonOptions)!;
    }

    public async Task<DepartmentDto> GetByIdAsync(long id)
    {
        HttpResponseMessage response = await _client.GetAsync($"departments/{id}");
        string content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
            throw new Exception(content);

        return JsonSerializer.Deserialize<DepartmentDto>(content, _jsonOptions)!;
    }

    public async Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto)
    {
        HttpResponseMessage httpResponse = await _client.PostAsJsonAsync("departments", dto);
        string content = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
            throw new Exception(content);

        return JsonSerializer.Deserialize<DepartmentDto>(content, _jsonOptions)!;
    }

    public async Task<DepartmentDto> UpdateAsync(long id, UpdateDepartmentDto dto)
    {
        HttpResponseMessage httpResponse =
            await _client.PutAsJsonAsync($"departments/{id}", dto);

        string content = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
            throw new Exception(content);

        return JsonSerializer.Deserialize<DepartmentDto>(content, _jsonOptions)!;
    }

    public async Task DeleteAsync(long id)
    {
        HttpResponseMessage httpResponse = await _client.DeleteAsync($"departments/{id}");
        string content = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
            throw new Exception(content);
    }

    public async Task<ICollection<DepartmentDto>> GetByTypeAsync(DepartmentType type)
    {
        HttpResponseMessage response = await _client.GetAsync($"departments/type/{type}");
        string content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
            throw new Exception(content);

        return JsonSerializer.Deserialize<List<DepartmentDto>>(content, _jsonOptions)!;
    }

    public async Task<ICollection<CowDto>> GetCowsByDepartmentAsync(long deptId)
    {
        HttpResponseMessage response = await _client.GetAsync($"departments/{deptId}/cows");
        string content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
            throw new Exception(content);

        return JsonSerializer.Deserialize<List<CowDto>>(content, _jsonOptions)!;
    }

    public async Task<ICollection<TransferRecordDto>> GetTransferRecordsByDepartmentAsync(long deptId)
    {
        HttpResponseMessage response = await _client.GetAsync($"departments/{deptId}/transfers");
        string content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
            throw new Exception(content);

        return JsonSerializer.Deserialize<List<TransferRecordDto>>(content, _jsonOptions)!;
    }

}
