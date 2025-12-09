// File: Server/WoM_WebApi/Services/Implementation/GrpcAuthServiceImpl.cs
using ApiContracts;
using Sep3.WayOfMilk.Grpc;
using WoM_WebApi.GlobalExceptionHandler;
using WoM_WebApi.Mapping;
using WoM_WebApi.Services.Interfaces;

namespace WoM_WebApi.Services.Implementation;

// Impl: gRPC-based auth
public class GrpcAuthServiceImpl : IAuthService
{
    private readonly UserService.UserServiceClient _client;

    public GrpcAuthServiceImpl(UserService.UserServiceClient client)
    {
        _client = client;
    }

    // Authenticate via gRPC
    public async Task<AuthenticatedUserDto> AuthenticateAsync(LoginRequestDto request)
    {
        var grpcRequest = new AuthenticationRequest
        {
            Email = request.Email,
            Password = request.Password
        };

        var reply = await _client.AuthenticateUserAsync(grpcRequest);

        if (reply == null || reply.Id == 0)
            throw new ValidationException("Invalid email or password.");

        return reply.ToAuthenticatedUser();
    }

    // Change own password
    public async Task ChangePasswordAsync(ChangePasswordDto request)
    {
        var grpcRequest = new ChangePasswordRequest
        {
            Id = request.UserId,
            OldPassword = request.OldPassword,
            NewPassword = request.NewPassword
        };

        var result = await _client.ChangePasswordAsync(grpcRequest);
        if (!result.Value)
            throw new ValidationException("Password change failed.");
    }

    // Owner reset password
    public async Task ResetPasswordAsync(ResetPasswordDto request)
    {
        var grpcRequest = new SentId { Id = request.UserId };
        await _client.ResetPasswordAsync(grpcRequest);
    }
}
