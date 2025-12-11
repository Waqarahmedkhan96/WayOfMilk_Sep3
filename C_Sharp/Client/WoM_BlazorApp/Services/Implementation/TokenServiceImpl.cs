using WoM_BlazorApp.Services.Interfaces;

namespace WoM_BlazorApp.Services.Implementation;

// In-memory token store
public class TokenServiceImpl : ITokenService
{
    public string? JwtToken { get; set; }
    //Ana’s login code (SimpleAuthProvider) will, after successful /auth/login, just do:

//_tokenService.JwtToken = loginResponse.Token;
}
