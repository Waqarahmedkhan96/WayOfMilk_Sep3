//Dummy Code for Path: Client/WoM_BlazorApp/Services/Implementation/SimpleAuthProvider.cs
namespace WoM_BlazorApp.Services.Implementation
using System.Security.Claims;
using System.Text.Json;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Authorization;
using ApiContracts.Authentication;
using ApiContracts.Users;
using Microsoft.JSInterop;

namespace BlazorApp.Services.Auth;

public class SimpleAuthProvider : AuthenticationStateProvider
{
    private readonly HttpClient httpClient;
    private readonly IJSRuntime jsRuntime;

    // Start as anonymous (no identity) — your original field 
    private ClaimsPrincipal current = new(new ClaimsIdentity());

    // Optional additional field 
    private ClaimsPrincipal? currentClaimsPrincipal;

     public SimpleAuthProvider(HttpClient httpClient, IJSRuntime jsRuntime)
    {
        this.httpClient = httpClient;
        this.jsRuntime = jsRuntime;
        // +ADD: nothing else needed here; jsRuntime is already injected for cache/restore
    }


    public override Task<AuthenticationState> GetAuthenticationStateAsync()
        => Task.FromResult(new AuthenticationState(current));

    // async login 
    public async Task LoginAsync(string userName, string password)
    {
        // LoginRequest uses set-properties; use object initializer:
        var request = new LoginRequest
        {
            UserName = userName,
            Password = password
        };

        HttpResponseMessage response = await httpClient.PostAsJsonAsync("auth/login", request);
        string content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception(content);

        var userDto = JsonSerializer.Deserialize<UserDto>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;

        // Build minimal claims from the returned user 
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, userDto.UserName),
            new("Id", userDto.Id.ToString()),
            new("perm", "create_post"),
            new("perm", "edit_own_post"),
            new("perm", "delete_own_post"),
            new("perm", "create_comment"),
            new("perm", "edit_own_comment"),
            new("perm", "delete_own_comment")
        };

        var identity = new ClaimsIdentity(claims, authenticationType: "Simple");
        current = new ClaimsPrincipal(identity);

        //I will Keep the optional field in sync 
        currentClaimsPrincipal = current;

        // Tell Blazor: auth state changed → refresh UI
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    //  Login wrapper 
    public Task Login(string userName, string password)
        => LoginAsync(userName, password);

    //  Logout method
    public void Logout()
    {
        current = new ClaimsPrincipal(new ClaimsIdentity());          // back to anonymous
        currentClaimsPrincipal = current;                              // keep both in sync
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    // ------------------------------
    // +ADD: Caching & Restore helpers
    // ------------------------------

    // +ADD: Login + cache helper (keeps your existing methods untouched).
    //       Call this instead of Login(...) if you want automatic caching after login.
    public async Task LoginWithCacheAsync(string userName, string password)
    {
        await LoginAsync(userName, password);

        // Cache the logged-in user in sessionStorage so refresh/back keeps you signed in
        try
        {
            var dto = GetCurrentUserDtoOrNull();
            if (dto is not null)
            {
                var json = JsonSerializer.Serialize(dto);
                await jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "currentUser", json);
            }
        }
        catch
        {
            // Ignore cache errors; auth state is already set in memory.
        }
    }

    // +ADD: async logout that also clears sessionStorage (your original Logout() stays as-is)
    public async Task LogoutAsync()
    {
        try
        {
            await jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "currentUser", "");
        }
        catch { /* ignore */ }

        // reuse your existing behavior
        Logout();
    }

    // +ADD: get current user as DTO from claims (for convenience)
    public UserDto? GetCurrentUserDtoOrNull()
    {
        if (current?.Identity is null || !current.Identity.IsAuthenticated) return null;

        var name = current.Identity?.Name ?? "";
        var idClaim = current.Claims.FirstOrDefault(c => c.Type == "Id")?.Value
                   ?? current.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(idClaim)) return null;
        if (!int.TryParse(idClaim, out var id)) return null;

        return new UserDto { Id = id, UserName = name };
    }

    // +ADD: set the auth state directly from a UserDto (no server call)
    public void SetUserFromDto(UserDto dto)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, dto.UserName),
            new("Id", dto.Id.ToString()),
            // keep parity with your existing permissions
            new("perm", "create_post"),
            new("perm", "edit_own_post"),
            new("perm", "delete_own_post"),
            new("perm", "create_comment"),
            new("perm", "edit_own_comment"),
            new("perm", "delete_own_comment")
        };

        var identity = new ClaimsIdentity(claims, authenticationType: "Simple");
        current = new ClaimsPrincipal(identity);
        currentClaimsPrincipal = current;

        // Tell Blazor: auth state changed → refresh UI
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    // +ADD: restore from sessionStorage (call once at startup if you want automatic restore)
    public async Task RestoreFromSessionAsync()
    {
        try
        {
            var json = await jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "currentUser");
            if (string.IsNullOrWhiteSpace(json)) return;

            var dto = JsonSerializer.Deserialize<UserDto>(json);
            if (dto is null) return;

            SetUserFromDto(dto);
        }
        catch
        {
            // If JS runtime isn't ready (first render), just skip; user stays anonymous
        }
    }
}



//I assume you(ANA) place your SimpleAuthProvider somewhere like WoM_BlazorApp.Services.Implementation.