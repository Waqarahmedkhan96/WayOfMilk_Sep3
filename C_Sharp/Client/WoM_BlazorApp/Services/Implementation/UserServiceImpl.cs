using System.Text.Json;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using ApiContracts;
using WoM_BlazorApp.Services.Interfaces;

namespace WoM_BlazorApp.Services.Implementation;

public class UserServiceImpl : IUserService
{
    private readonly HttpClient _client;

    // We use a custom JsonSerializerOptions to handle case-insensitivity (API returns camelCase)
    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() } // <--- converter here
    };

    public UserServiceImpl(HttpClient client)
    {
        _client = client;
    }

    // READ
    public async Task<UserDto> GetCurrentUserAsync()
    {
        // GET /users/current-user
        var response = await _client.GetAsync("users/current-user");
        await HandleErrors(response);

        return await response.Content.ReadFromJsonAsync<UserDto>(_options)
               ?? throw new Exception("Failed to parse user data");
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        // GET /users
        var response = await _client.GetAsync("users");
        await HandleErrors(response);

        return await response.Content.ReadFromJsonAsync<IEnumerable<UserDto>>(_options)
               ?? new List<UserDto>();
    }

    public async Task<UserDto> GetByIdAsync(long id)
    {
        // GET /users/{id}
        var response = await _client.GetAsync($"users/{id}");
        await HandleErrors(response);

        return await response.Content.ReadFromJsonAsync<UserDto>(_options)
               ?? throw new Exception("User not found");
    }

    // CREATE
    public async Task CreateAsync(CreateUserDto dto)
    {
        // POST /users
        var response = await _client.PostAsJsonAsync("users", dto);
        await HandleErrors(response);
    }

    // UPDATE
    public async Task UpdateAsync(long id, UpdateUserDto dto)
    {
        // PUT /users/{id}
        var response = await _client.PutAsJsonAsync($"users/{id}", dto);
        await HandleErrors(response);
    }

    public async Task UpdateProfileAsync(UpdateUserDto dto)
    {
        // PUT /users/profile
        var response = await _client.PutAsJsonAsync("users/profile", dto);
        await HandleErrors(response);
    }

    // DELETE
    public async Task DeleteAsync(long id)
    {
        // DELETE /users/{id}
        var response = await _client.DeleteAsync($"users/{id}");
        await HandleErrors(response);
    }

    //helper
    private async Task HandleErrors(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception(string.IsNullOrEmpty(error) ? response.ReasonPhrase : error);
        }
    }

}