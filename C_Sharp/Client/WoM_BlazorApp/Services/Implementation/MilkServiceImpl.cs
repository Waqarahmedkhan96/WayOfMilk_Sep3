using System.Net.Http.Json;
using System.Net.Http.Headers;
using ApiContracts;
using WoM_BlazorApp.Services.Interfaces;

namespace WoM_BlazorApp.Services.Implementation;

// Calls /milk endpoints
public class MilkServiceImpl : IMilkService
{
    private readonly HttpClient _http;
    private readonly ITokenService _tokenService;

    public MilkServiceImpl(HttpClient http, ITokenService tokenService)
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

    public async Task<MilkListDto> GetAllAsync()
    {
        AttachToken(); // add jwt
        var result = await _http.GetFromJsonAsync<MilkListDto>("milk");
        return result ?? new MilkListDto();
    }

    public async Task<MilkDto> GetByIdAsync(long id)
    {
        AttachToken(); // add jwt
        var result = await _http.GetFromJsonAsync<MilkDto>($"milk/{id}");
        return result!;
    }

    public async Task<MilkListDto> GetByContainerAsync(long id)
    {
        AttachToken(); // add jwt
        var result = await _http.GetFromJsonAsync<MilkListDto>($"milk/by-container/{id}");
        return result ?? new MilkListDto();
    }

    public async Task<MilkDto> CreateAsync(CreateMilkDto dto)
    {
        AttachToken(); // add jwt
        var response = await _http.PostAsJsonAsync("milk", dto);
        response.EnsureSuccessStatusCode();
        var created = await response.Content.ReadFromJsonAsync<MilkDto>();
        return created!;
    }

    public async Task<MilkDto> UpdateAsync(long id, UpdateMilkDto dto)
    {
        AttachToken(); // add jwt
        var response = await _http.PutAsJsonAsync($"milk/{id}", dto);
        response.EnsureSuccessStatusCode();
        var updated = await response.Content.ReadFromJsonAsync<MilkDto>();
        return updated!;
    }

    public async Task ApproveAsync(long id, bool approved)
    {
        AttachToken(); // add jwt

        var dto = new ApproveMilkStorageDto
        {
            // ApprovedByUserId ignored in WebApi, taken from JWT
            ApprovedForStorage = approved
        };

        var response = await _http.PostAsJsonAsync($"milk/{id}/approve", dto);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(long id)
    {
        AttachToken(); // add jwt
        var response = await _http.DeleteAsync($"milk/{id}");
        response.EnsureSuccessStatusCode();
    }
}
