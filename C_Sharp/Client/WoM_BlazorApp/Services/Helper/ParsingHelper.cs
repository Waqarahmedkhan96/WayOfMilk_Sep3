using System.Security.Claims;
using System.Text.Json;

namespace WoM_BlazorApp.Services.Helper;

public static class ParsingHelper
{
    public static IEnumerable<Claim> ParseClaimsFromJwt(this string jwt)
    {
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

        var claims = new List<Claim>();

        if (keyValuePairs != null)
        {
            foreach (var kvp in keyValuePairs)
            {
                if (kvp.Value is JsonElement element && element.ValueKind == JsonValueKind.Array)
                {
                    foreach (var item in element.EnumerateArray()) claims.Add(new Claim(kvp.Key, item.ToString()));
                }
                else claims.Add(new Claim(kvp.Key, kvp.Value.ToString()!));
            }
        }
        return claims;
    }

    public static byte[] ParseBase64WithoutPadding(this string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }
    
}