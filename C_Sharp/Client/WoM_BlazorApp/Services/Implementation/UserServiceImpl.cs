using System.Text.Json;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using ApiContracts;
using WoM_BlazorApp.Services.Interfaces;

namespace WoM_BlazorApp.Services.Implementation;

public class UserServiceImpl : IUserService
{
    private readonly HttpClient _client;

    // Options to handle Enum "Worker" vs Integer 0 mismatch
    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    // FIX: Inject HttpClient directly (Program.cs handles the wiring now)
    public UserServiceImpl(HttpClient client)
    {
        _client = client;
    }

    // READ
    public async Task<UserDto> GetCurrentUserAsync()
    {
        var response = await _client.GetAsync("users/current-user");
        await HandleErrors(response);

        return await response.Content.ReadFromJsonAsync<UserDto>(_options)
               ?? throw new Exception("Failed to parse user data");
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        var response = await _client.GetAsync("users");
        await HandleErrors(response);

        return await response.Content.ReadFromJsonAsync<IEnumerable<UserDto>>(_options)
               ?? new List<UserDto>();
    }

    public async Task<UserDto> GetByIdAsync(long id)
    {
        var response = await _client.GetAsync($"users/{id}");
        await HandleErrors(response);

        return await response.Content.ReadFromJsonAsync<UserDto>(_options)
               ?? throw new Exception("User not found");
    }

    // CREATE
    public async Task CreateAsync(CreateUserDto dto)
    {
        // Pass options to ensure Enums are serialized as Strings
        var response = await _client.PostAsJsonAsync("users", dto, _options);
        await HandleErrors(response);
    }

    // UPDATE
    public async Task UpdateAsync(long id, UpdateUserDto dto)
    {
        var response = await _client.PutAsJsonAsync($"users/{id}", dto, _options);
        await HandleErrors(response);
    }

    public async Task UpdateProfileAsync(UpdateUserDto dto)
    {
        var response = await _client.PutAsJsonAsync("users/profile", dto, _options);
        await HandleErrors(response);
    }

    // DELETE
    public async Task DeleteAsync(long id)
    {
        var response = await _client.DeleteAsync($"users/{id}");
        await HandleErrors(response);
    }

    private async Task HandleErrors(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception(string.IsNullOrEmpty(error) ? response.ReasonPhrase : error);
        }
    }
}