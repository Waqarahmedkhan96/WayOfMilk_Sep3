using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using ApiContracts;
using WoM_BlazorApp.Services.Interfaces;

namespace WoM_BlazorApp.Services.Implementation;

public class DepartmentServiceImpl : IDepartmentService
{
    private readonly HttpClient _client;

    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    public DepartmentServiceImpl(HttpClient client)
    {
        _client = client;
    }

    public async Task<ICollection<DepartmentDto>> GetAllAsync()
    {
        var response = await _client.GetAsync("departments");
        response.EnsureSuccessStatusCode();

        var wrapper = await response.Content
            .ReadFromJsonAsync<DepartmentListDto>(_options);

        return wrapper?.Departments ?? new List<DepartmentDto>();
    }

    public async Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto)
    {
        var response = await _client.PostAsJsonAsync("departments", dto, _options);
        response.EnsureSuccessStatusCode();

        var created = await response.Content
            .ReadFromJsonAsync<DepartmentDto>(_options);

        return created!;
    }

    public async Task<DepartmentDto> UpdateAsync(long id, UpdateDepartmentDto dto)
    {
        var response = await _client.PutAsJsonAsync($"departments/{id}", dto, _options);
        response.EnsureSuccessStatusCode();

        var updated = await response.Content
            .ReadFromJsonAsync<DepartmentDto>(_options);

        return updated!;
    }

    public async Task DeleteAsync(long id)
    {
        var response = await _client.DeleteAsync($"departments/{id}");
        response.EnsureSuccessStatusCode();
    }
}
