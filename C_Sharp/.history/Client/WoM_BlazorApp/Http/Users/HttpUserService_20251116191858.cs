using System.Net.Http.Json;
using System.Text.Json;
using ApiContracts.Users;

namespace WoM_BlazorApp.Http;

public class HttpUserService : IUserService
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public HttpUserService(HttpClient client)
    {
        _client = client;
    }

    public async Task<ICollection<UserDto>> GetAllAsync()
    {
        HttpResponseMessage response = await _client.GetAsync("users");
        string content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
            throw new Exception(content);

        var users = JsonSerializer.Deserialize<List<UserDto>>(content, _jsonOptions)!;
        return users;
    }

    public async Task<UserDto> GetByIdAsync(int id)
    {
        HttpResponseMessage response = await _client.GetAsync($"users/{id}");
        string content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
            throw new Exception(content);

        return JsonSerializer.Deserialize<UserDto>(content, _jsonOptions)!;
    }

    public async Task<UserDto> CreateAsync(CreateUserDto dto)
    {
        HttpResponseMessage httpResponse = await _client.PostAsJsonAsync("users", dto);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
            throw new Exception(response);

        return JsonSerializer.Deserialize<UserDto>(response, _jsonOptions)!;
    }

    public async Task UpdateAsync(int id, UpdateUserDto dto)
    {
        HttpResponseMessage httpResponse = await _client.PutAsJsonAsync($"users/{id}", dto);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
            throw new Exception(response);
    }

    public async Task DeleteAsync(int id)
    {
        HttpResponseMessage httpResponse = await _client.DeleteAsync($"users/{id}");
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
            throw new Exception(response);
    }
}
