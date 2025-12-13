//Dummy Code for Path: Client/WoM_BlazorApp/Services/Implementation/SimpleAuthProvider.cs

using WoM_BlazorApp.Services.Helper;
using WoM_BlazorApp.Services.Interfaces;

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
    public async Task LoginAsync(string email, string password)
    {
        // LoginRequest uses set-properties; use object initializer:
        var request = new LoginRequestDto
        {
            Email = email,
            Password = password
        };

        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("auth/login", request);
        if (!response.IsSuccessStatusCode)
        {
            string errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Login failed: {errorContent}");
        }

        // Deserialize correct DTO (LoginResponseDto)
        var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponseDto>(new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (loginResponse == null) throw new Exception("Login response was empty.");

        // 3. Process the login (Update headers and state)
        await HandleLoginSuccess(loginResponse);
    }
    private async Task HandleLoginSuccess(LoginResponseDto dto)
    {
        // Set the JWT in the HTTP Client so all future requests use it
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", dto.Token);
        //sync the token service with the new token
        _tokenService.JwtToken = dto.Token;


        var claims = dto.Token.ParseClaimsFromJwt();

        // Create the Identity
        var identity = new ClaimsIdentity(claims, "jwt");
        _currentUser = new ClaimsPrincipal(identity);

        // Cache the result so we stay logged in on refresh
        // We store the whole DTO as JSON string
        string json = JsonSerializer.Serialize(dto);
        await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "currentUser", json);

        // Notify Blazor
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    // ----------------------------------------
    // LOGOUT
    // ----------------------------------------
    public async Task LogoutAsync()
    {
        // Clear Headers
        _httpClient.DefaultRequestHeaders.Authorization = null;
        // Clear Token Service
        _tokenService.JwtToken = null;

        // Clear Cache
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
            if (string.IsNullOrWhiteSpace(json)) return;

            var dto = JsonSerializer.Deserialize<LoginResponseDto>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
                //preventive to avoid deserialization errors
            });

            if (dto == null) return;

            // Re-apply the login logic (set headers, parse claims, restore token)
            // Note: We duplicate logic slightly to avoid re-saving to session storage
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", dto.Token);
            _tokenService.JwtToken = dto.Token;

            var claims = dto.Token.ParseClaimsFromJwt();
            var identity = new ClaimsIdentity(claims, "jwt");
            _currentUser = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
        catch
        {
            // If restore fails, user is effectively logged out
        }
    }
}

