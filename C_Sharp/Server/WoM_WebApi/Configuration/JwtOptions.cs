namespace WoM_WebApi.Configuration;
// JWT config values
public class JwtOptions
{
    public string Key { get; set; } = string.Empty;      // signing key
    public string Issuer { get; set; } = string.Empty;   // token issuer
    public string Audience { get; set; } = string.Empty; // token audience
    public int ExpiresMinutes { get; set; } = 60;        // token lifetime

    // Change the key to something long before real use, In appsettings.jason.
}
