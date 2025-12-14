using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using ApiContracts;

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
}
