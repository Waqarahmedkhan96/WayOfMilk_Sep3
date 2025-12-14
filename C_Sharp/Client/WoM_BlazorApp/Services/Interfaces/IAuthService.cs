using ApiContracts;

namespace WoM_BlazorApp.Services.Interfaces;

public interface IAuthService
{
    Task ChangePasswordAsync(ChangePasswordDto dto);
    Task ResetPasswordAsync(ResetPasswordDto dto);
}