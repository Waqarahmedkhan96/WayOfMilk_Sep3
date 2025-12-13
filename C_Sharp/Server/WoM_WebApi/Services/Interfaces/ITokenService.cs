using ApiContracts;

namespace WoM_WebApi.Services.Interfaces;

// Interface: JWT token operations
public interface ITokenService
{
    string GenerateToken(AuthenticatedUserDto user); // build JWT
}
