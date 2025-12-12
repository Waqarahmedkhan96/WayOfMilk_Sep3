using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace WoM_WebApi.Helper;

public static class IdentityHelper
{
    //named methods special so we know where they come from
    public static long GetJWTId(this ClaimsPrincipal user)
    {
        var sub = user.FindFirst(ClaimTypes.NameIdentifier)
                  ?? user.FindFirst(JwtRegisteredClaimNames.Sub);

        if (sub != null && long.TryParse(sub.Value, out var id))
        {
            return id;
        }
        return 0; // Return 0 if not found or invalid
    }

    //get user role
    public static string? GetJWTRole(this ClaimsPrincipal user)
    {
        var roleClaim = user.FindFirst(ClaimTypes.Role)
                        ?? user.FindFirst("role"); // Fallback for some JWT providers

        return roleClaim?.Value;
    }

    //get user email
    public static string? GetJWTEmail(this ClaimsPrincipal user)
    {
        var emailClaim = user.FindFirst(ClaimTypes.Email)
                        ?? user.FindFirst("email"); // Fallback for some JWT providers
        return emailClaim?.Value;
    }

    //get user name
    public static string? GetJWTName(this ClaimsPrincipal user)
    {
        var nameClaim = user.FindFirst(ClaimTypes.Name)
                        ?? user.FindFirst(JwtRegisteredClaimNames.Name)
                        ?? user.FindFirst("name"); // Fallback for some JWT providers
        return nameClaim?.Value;
    }

    // Formatted Name & ID (For Logging)
    public static string GetJWTNameAndId(this ClaimsPrincipal user)
    {
        var idClaim = user.FindFirst(ClaimTypes.NameIdentifier)
                      ?? user.FindFirst(JwtRegisteredClaimNames.Sub);

        var nameClaim = user.FindFirst(ClaimTypes.Name)
                        ?? user.FindFirst(JwtRegisteredClaimNames.Name)
                        ?? user.FindFirst("name");

        if (idClaim != null)
        {
            var name = nameClaim?.Value ?? "Unknown";
            return $"{name} id {idClaim.Value}";
        }

        return "Anonymous";
    }
}