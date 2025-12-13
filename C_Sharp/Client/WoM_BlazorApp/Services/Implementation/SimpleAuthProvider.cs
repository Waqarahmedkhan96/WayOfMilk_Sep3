using WoM_BlazorApp.Services.Helper;
using WoM_BlazorApp.Services.Interfaces;
using System.Text.Json.Serialization;

namespace WoM_BlazorApp.Services.Implementation;
using System.Security.Claims;
using System.Text.Json;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Authorization;
using ApiContracts;
using Microsoft.JSInterop;
using System.Net.Http.Headers; // Needed to set Authorization header


public class SimpleAuthProvider : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _jsRuntime;
    private readonly ITokenService _tokenService;

    // We store the current user here
    private ClaimsPrincipal _currentUser = new(new ClaimsIdentity());


     public SimpleAuthProvider(HttpClient httpClient, IJSRuntime jsRuntime, ITokenService tokenService)
    {
        _httpClient = httpClient;
        _jsRuntime = jsRuntime;
        _tokenService = tokenService;
    }


    public override Task<AuthenticationState> GetAuthenticationStateAsync()
        => Task.FromResult(new AuthenticationState(_currentUser));

    // async login
    public async Task LoginWithCacheAsync(string email, string password)
    {
        var loginResponse = await LoginAsync(email, password);

        // Create options to write Enums as Strings
        var options = new JsonSerializerOptions();
        options.Converters.Add(new JsonStringEnumConverter());

        // Serialize with options
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
            PropertyNameCaseInsensitive = true
        };
        options.Converters.Add(new JsonStringEnumConverter()); // To handle UserRole enum

        var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponseDto>(options);

        if (loginResponse == null) throw new Exception("Login response was empty.");

        UpdateAuthenticationState(loginResponse);
        return loginResponse;
    }

    // ----------------------------------------
    // LOGOUT
    // ----------------------------------------
    public async Task LogoutAsync()
    {
        // Clear Token Service
        _tokenService.JwtToken = null;

        // Clear Browser Cache
        await _jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", "currentUser");

        // Reset State to Anonymous
        _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    // ----------------------------------------
    // AUTO-RESTORE (Refresh Page)
    // ----------------------------------------
    // Call this in your MainLayout.razor or App.razor OnInitializedAsync
    public async Task RestoreSessionAsync()
    {
        try
        {
            var json = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "currentUser");
            if (string.IsNullOrWhiteSpace(json))
                return;

            var dto = JsonSerializer.Deserialize<LoginResponseDto>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
            });


            if (dto == null || string.IsNullOrEmpty(dto.Token))
                return;

            if ((dto.Token).IsTokenExpired())
            {
                Console.WriteLine("Token expired during restore. Logging out.");
                await LogoutAsync(); // Clean up storage
                return;
            }

            UpdateAuthenticationState(dto);
        }
        catch
        {
            // If restore fails (e.g. invalid JSON), clear everything
            await LogoutAsync();
        }
    }

    // ------------------------------------------------------------------------
    // Helper: Update State
    // ------------------------------------------------------------------------
    private void UpdateAuthenticationState(LoginResponseDto dto)
    {
        Console.WriteLine($"[AuthProvider] Setting Token: {dto.Token.Substring(0, 10)}...");
        // Sync Token Service
        _tokenService.JwtToken = dto.Token;

        // Parse Claims & Update Principal
        // Uses the ParsingHelper extension method
        var claims = dto.Token.ParseClaimsFromJwt();

        var identity = new ClaimsIdentity(
            claims,
            "jwt",
            // Map .Identity.Name to the standard long URL claim
            nameType: ClaimTypes.Name,
            // Map .IsInRole() to the standard long URL claim
            roleType: ClaimTypes.Role
        );

        _currentUser = new ClaimsPrincipal(identity);

        // Notify Blazor to re-render
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}

