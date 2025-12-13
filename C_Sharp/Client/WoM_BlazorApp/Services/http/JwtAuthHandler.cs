using System.Net.Http.Headers;
using WoM_BlazorApp.Services.Helper;
using WoM_BlazorApp.Services.Interfaces;

namespace WoM_BlazorApp.Services.Http;

public class JwtAuthHandler : DelegatingHandler
{
    private readonly ITokenService _tokenService;

    public JwtAuthHandler(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = _tokenService.JwtToken;

        // 1. Check if token exists AND is expired
        if (!string.IsNullOrWhiteSpace(token) && token.IsTokenExpired())
        {
            // If expired, force a logout (requires injecting AuthProvider here, or throwing an exception)
            // Ideally, you would throw a specific exception that the UI catches to redirect to login.
            throw new UnauthorizedAccessException("Token expired.");
        }

        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}