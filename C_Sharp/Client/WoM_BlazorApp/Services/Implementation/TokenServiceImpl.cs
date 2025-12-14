using WoM_BlazorApp.Services.Interfaces;

namespace WoM_BlazorApp.Services.Implementation;

// In-memory token store
public class TokenServiceImpl : ITokenService
{
    public string? JwtToken { get; set; }

}
//just making triple sure this working version gets extra pushed