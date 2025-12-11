namespace WoM_BlazorApp.Services.Interfaces;

// Simple token store (scoped)
public interface ITokenService
{
    string? JwtToken { get; set; }   // current JWT
}
