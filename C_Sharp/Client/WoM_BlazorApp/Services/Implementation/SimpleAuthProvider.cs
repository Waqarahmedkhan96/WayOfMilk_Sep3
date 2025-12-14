using WoM_BlazorApp.Services.Helper;
using WoM_BlazorApp.Services.Interfaces;
using System.Text.Json.Serialization;
using System.Security.Claims;
using System.Text.Json;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Authorization;
using ApiContracts;
using Microsoft.JSInterop;

namespace WoM_BlazorApp.Services.Implementation;

public class SimpleAuthProvider : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _jsRuntime;
    private readonly ITokenService _tokenService;

    private ClaimsPrincipal _currentUser = new(new ClaimsIdentity());

    public SimpleAuthProvider(HttpClient httpClient, IJSRuntime jsRuntime, ITokenService tokenService)
    {
        _httpClient = httpClient;
        _jsRuntime = jsRuntime;
        _tokenService = tokenService;
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
        => Task.FromResult(new AuthenticationState(_currentUser));

    public async Task LoginWithCacheAsync(string email, string password)
    {
        var loginResponse = await LoginAsync(email, password);

        // Serialize with options (Enum as String) for Storage
        var options = new JsonSerializerOptions();
        options.Converters.Add(new JsonStringEnumConverter());
        string json = JsonSerializer.Serialize(loginResponse, options);

        await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "currentUser", json);
    }

    public async Task<LoginResponseDto> LoginAsync(string email, string password)
    {
        var request = new LoginRequestDto { Email = email, Password = password };
        var response = await _httpClient.PostAsJsonAsync("auth/login", request);

        if (!response.IsSuccessStatusCode)
        {
            string errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Login failed: {errorContent}");
        }

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() }
        };

        var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponseDto>(options);
        if (loginResponse == null) throw new Exception("Login response was empty.");

        UpdateAuthenticationState(loginResponse);
        return loginResponse;
    }

    public async Task LogoutAsync()
    {
        _tokenService.JwtToken = null;
        await _jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", "currentUser");
        _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task RestoreSessionAsync()
    {
        try
        {
            var json = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "currentUser");
            if (string.IsNullOrWhiteSpace(json)) return;

            var dto = JsonSerializer.Deserialize<LoginResponseDto>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
            });

            if (dto == null || string.IsNullOrEmpty(dto.Token)) return;

            // Check expiry (Using your helper)
            if (dto.Token.IsTokenExpired())
            {
                await LogoutAsync();
                return;
            }

            UpdateAuthenticationState(dto);
        }
        catch
        {
            await LogoutAsync();
        }
    }

    private void UpdateAuthenticationState(LoginResponseDto dto)
    {
        // CRITICAL: Ensure TokenService has the token for the Handler to use
        _tokenService.JwtToken = dto.Token;

        var claims = dto.Token.ParseClaimsFromJwt();
        var identity = new ClaimsIdentity(
            claims,
            "jwt",
            nameType: ClaimTypes.Name,
            roleType: ClaimTypes.Role
        );

        _currentUser = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}