using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using ApiContracts;
using WoM_BlazorApp.Services.Interfaces;

namespace WoM_BlazorApp.Services.Implementation;

public class AuthServiceImpl(HttpClient client) : IAuthService
{
    //converted to primary constructor (see above: ClassName(ParameterType other)
    /*
    private readonly HttpClient _client;

    public AuthServiceImpl(HttpClient client) =>
        _client = client;
        */

    public async Task ChangePasswordAsync(ChangePasswordDto dto)
    {
        // This requires [Authorize], so the token is mandatory
        var response = await client.PostAsJsonAsync("auth/change-password", dto);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception(error);
        }
    }

    public async Task ResetPasswordAsync(ResetPasswordDto dto)
    {
        // This requires [Authorize(Roles="Owner")], so the token is mandatory
        var response = await client.PostAsJsonAsync("auth/reset-password", dto);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception(error);
        }
    }
}