using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Text.Json;
using ApiContracts;
using WoM_BlazorApp.Services.Interfaces;

namespace WoM_BlazorApp.Services.Implementation;

// Calls /containers endpoints
public class ContainerServiceImpl : IContainerService
{
    private readonly HttpClient _http;
    private readonly ITokenService _tokenService;
    //this function is used to deserialize the json response from the server (token relevant)
    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true
    };
    public ContainerServiceImpl(HttpClient http, ITokenService tokenService)
    {
        _http = http;
        _tokenService = tokenService;
    }

    // attach JWT header
    private void AttachToken()
    {
        var token = _tokenService.JwtToken;
        if (!string.IsNullOrWhiteSpace(token))
        {
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task<ContainerListDto> GetAllAsync()
    {
        AttachToken(); // add jwt
        var result = await _http.GetFromJsonAsync<ContainerListDto>("containers");
        return result ?? new ContainerListDto();
    }



    public async Task<ContainerDto> GetByIdAsync(long id)
    {
        AttachToken(); // add jwt
        var result = await _http.GetFromJsonAsync<ContainerDto>($"containers/{id}");
        return result!;
    }

    public async Task<ContainerDto> CreateAsync(CreateContainerDto dto)
    {
        AttachToken(); // add jwt
        var response = await _http.PostAsJsonAsync("containers", dto);
        response.EnsureSuccessStatusCode();
        var created = await response.Content.ReadFromJsonAsync<ContainerDto>();
        return created!;
    }

    public async Task<ContainerDto> UpdateAsync(long id, UpdateContainerDto dto)
    {
        AttachToken(); // add jwt
        var response = await _http.PutAsJsonAsync($"containers/{id}", dto);
        response.EnsureSuccessStatusCode();
        var updated = await response.Content.ReadFromJsonAsync<ContainerDto>();
        return updated!;
    }

    public async Task DeleteAsync(long id)
    {
        AttachToken(); // add jwt
        var response = await _http.DeleteAsync($"containers/{id}");
        response.EnsureSuccessStatusCode();
    }

    //added for mock tracing
    public async Task<IEnumerable<ContainerDto>> GetAllTrackedAsync()
    {
        var result = await GetAllAsync();
        var response = await _http.GetAsync("cows");
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(await response.Content.ReadAsStringAsync());
        }
        return await response.Content.ReadFromJsonAsync<IEnumerable<ContainerDto>>(_options)
               ?? new List<ContainerDto>();
    }
}
