using ApiContracts;

namespace WoM_WebApi.Services.Interfaces;

// Interface: authentication service
public interface IAuthService
{
    Task<AuthenticatedUserDto> AuthenticateAsync(LoginRequestDto request); // login
    Task ChangePasswordAsync(ChangePasswordDto request);                   // change own
    Task ResetPasswordAsync(ResetPasswordDto request);                     // owner reset
}
