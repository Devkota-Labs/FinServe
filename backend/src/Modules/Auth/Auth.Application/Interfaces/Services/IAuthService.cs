using Auth.Application.Dtos;
using Shared.Application.Results;

namespace Auth.Application.Interfaces.Services;

public interface IAuthService
{
    // Register
    Task<Result<RegisterResponseDto>> RegisterAsync(RegisterRequestDto dto, CancellationToken cancellationToken);

    // Email verification
    Task<Result> VerifyEmailAsync(VerifyEmailDto dto, CancellationToken cancellationToken);
    Task<Result> SendVerificationMailAsync(int userId, CancellationToken cancellationToken);

    // Email & mobile updates
    Task<Result> UpdateEmailAsync(int userId, UpdateEmailDto dto, CancellationToken cancellationToken);
    Task<Result> UpdateMobileAsync(int userId, UpdateMobileDto dto, CancellationToken cancellationToken);

    // Generic OTP
    Task<Result> SendOtpAsync(int userId, SendOtpDto dto, CancellationToken cancellationToken);
    Task<Result> VerifyOtpAsync(int userId, VerifyOtpDto dto, CancellationToken cancellationToken);

    // Auth tokens
    Task<Result<LoginResponseDto>> LoginAsync(LoginDto dto, string ipAddress, CancellationToken cancellationToken);
    Task<Result<LoginResponseDto>> RefreshAsync(string refreshToken, string ipAddress, CancellationToken cancellationToken);
    Task<Result> LogoutAsync(string refreshToken, string ipAddress, CancellationToken cancellationToken);

    // Password flows
    Task<Result> SendForgotPasswordAsync(ForgotPasswordDto dto, CancellationToken cancellationToken);
    Task<Result> ResetPasswordAsync(ResetPasswordDto dto, CancellationToken cancellationToken);
    Task<Result> ChangePasswordAsync(int userId, ChangePasswordDto dto, CancellationToken cancellationToken);
}
