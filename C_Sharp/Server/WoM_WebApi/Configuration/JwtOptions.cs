namespace WoM_WebApi.Configuration;

// JWT config options
public class JwtOptions
{
    public string Key { get; set; } = string.Empty;      // symmetric key
    public string Issuer { get; set; } = string.Empty;   // token issuer
    public string Audience { get; set; } = string.Empty; // token audience
    public int ExpiresMinutes { get; set; } = 60;        // lifetime
}
