using ApiContracts;
using Sep3.WayOfMilk.Grpc;
using WoM_WebApi.GlobalExceptionHandler;
using WoM_WebApi.Log;
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
        //logging the attempt
        ActivityLog.Instance.Log("Login", "User trying to log in", request.Email);
        var grpcRequest = new AuthenticationRequest
        {
            Email    = request.Email,
            Password = request.Password
        };

        var reply = await _client.AuthenticateUserAsync(grpcRequest);

        if (reply == null || reply.Id == 0)
        {
            ActivityLog.Instance.Log("Login", "Invalid email or password", request.Email);
            throw new ValidationException("Invalid email or password.");

        }

        ActivityLog.Instance.Log("Login Success", $"User {reply.Name} logged in successfully", reply.Name, reply.Id);
        // extension method from UserGrpcMapper
        return reply.ToAuthenticatedUser();
    }

    // Change own password
    public async Task ChangePasswordAsync(ChangePasswordDto request)
    {
        ActivityLog.Instance.Log("Change Password", $"User with ID {request.UserId} is trying to change password", "", request.UserId);
        var grpcRequest = new ChangePasswordRequest
        {
            Id          = request.UserId,
            OldPassword = request.OldPassword,
            NewPassword = request.NewPassword
        };


        var result = await _client.ChangePasswordAsync(grpcRequest);
        if (!result.Value)
        {
            ActivityLog.Instance.Log("Change Password Failed", $"User with ID {request.UserId} failed to change password", "", request.UserId);
                        throw new ValidationException("Password change failed.");
        }
        ActivityLog.Instance.Log("Change Password Success", $"User with ID {request.UserId} changed password successfully", "", request.UserId);

    }

    // Owner reset password
    public async Task ResetPasswordAsync(ResetPasswordDto request)
    {
        ActivityLog.Instance.Log("Reset Password", $"An admin is trying to reset password for user with ID {request.UserId}", "", request.UserId);
        var grpcRequest = new SentId { Id = request.UserId };
        await _client.ResetPasswordAsync(grpcRequest);
        //since this returns an error if it fails, we assume success if we reach here
        ActivityLog.Instance.Log("Reset Password Success", $"Password for user with ID {request.UserId} has been reset successfully", "", request.UserId);
    }
}
