using Auth.Application.Dtos;
using Auth.Application.Interfaces.Repositories;
using Auth.Application.Interfaces.Services;
using Auth.Application.Models;
using Auth.Application.Options;
using Auth.Domain.Entities;
using Lookup.Application.Lookups;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Serilog;
using Shared.Application.Interfaces.Services;
using Shared.Application.Results;
using Shared.Common;
using Shared.Common.Services;
using Shared.Common.Utils;
using Shared.Infrastructure.Options;
using Shared.Security;
using Shared.Security.Configurations;
using Users.Application.Dtos.User;
using Users.Application.Interfaces.Services;

namespace Auth.Application.Services;

internal sealed class AuthService(ILogger logger,
    IUserReadService usersRead, 
    IUserWriteService usersWrite, 
    IPasswordHasher passwordHasher, 
    IPasswordPolicyService passwordPolicyService,
    IPasswordHistoryService passwordHistoryService, 
    IEmailService emailService, 
    IEmailTemplateRenderer emailTemplateRenderer,
    IOptions<TokenOptions> tokenOptions,
    IOptions<OtpOptions> otpOptions,
    ISmsSender? smsSender, 
    IOtpGenerator otpGenerator, 
    IOtpRepository otpRepository, 
    IRefreshTokenRepository refreshTokens, 
    IAppUrlProvider appUrlProvider,
    IMfaService mfaService,
    IJwtTokenGenerator jwtTokenGenerator,
    IRefreshTokenService refreshTokenService,
    IMenuReadService menuReadService,
    IHttpContextAccessor httpContextAccessor,
    ILoginHistoryService loginHistoryService
    , IOptions<SecurityOptions> securityOptions
    , IOptions<AdminOptions> adminOptions
    , IOptions<ApiOptions> apiOptions
    , IOptions<FrontendOptions> frontendOption
    )
    : BaseService(logger.ForContext<AuthService>(), null), IAuthService
{
    private readonly TokenOptions _tokenOptions = tokenOptions.Value;
    private readonly OtpOptions _otpOptions = otpOptions.Value;
    private readonly SecurityOptions _securityOptions = securityOptions.Value;
    private readonly AdminOptions _adminOptions = adminOptions.Value;
    private readonly ApiOptions _apiOptions = apiOptions.Value;
    private readonly FrontendOptions _frontendOptions = frontendOption.Value;

    // ======================================================
    // 1. Register
    // ======================================================
    public async Task<Result> RegisterAsync(RegisterRequestDto dto, CancellationToken cancellationToken)
    {
        // uniqueness checks
        if (await usersRead.EmailExistsAsync(dto.Email, cancellationToken).ConfigureAwait(false))
            return Result.Fail("Email already exists.");

        if (await usersRead.MobileExistsAsync(dto.Mobile, cancellationToken).ConfigureAwait(false))
            return Result.Fail("Mobile already exists.");

        if (await usersRead.UserNameExistsAsync(dto.UserName, cancellationToken).ConfigureAwait(false))
            return Result.Fail("User Name already exists.");

        var (valid, message) = passwordPolicyService.ValidatePassword(dto.Password);
        if (!valid)
            return Result.Fail(message);

        var passwordHash = passwordHasher.Hash(dto.Password);

        if (!GenderLookup.TryFromCode(dto.Gender, out var gender))
        {
            return Result.Fail("Invalid gender");
        }

        var createUserDto = new CreateUserDto(dto.UserName, dto.Email, dto.Mobile, gender, dto.DateOfBirth, dto.FirstName, dto.MiddleName, dto.LastName, dto.Addresses, passwordHash);

        var authUserDto = await usersWrite.CreateUserAsync(createUserDto, cancellationToken).ConfigureAwait(false);

        //Adding password history
        await passwordHistoryService.SaveAsync(authUserDto.Id, passwordHash, cancellationToken).ConfigureAwait(false);

        // Send email verification
        var emailSendResult = await SendVerificationMailAsync(authUserDto.Id, cancellationToken).ConfigureAwait(false);

        if (emailSendResult.Success)
        {
            Logger.Information("Verification email sent to {Email}", dto.Email);
        }
        else
        {
            Logger.Warning("Failed to send verification email to {Email}", dto.Email);

            return Result.Fail($"Failed to send verification email to {dto.Email}");
        }

        var notificationEmailResult = await SendApprovalFollowUpMailAsync(authUserDto.Id, cancellationToken).ConfigureAwait(false);

        if (notificationEmailResult.Success)
        {
            Logger.Information("Verification email sent to {Email}", dto.Email);
        }
        else
        {
            Logger.Warning("Failed to send notification email to admin for user {Email}", dto.Email);
        }

        // Optional: send mobile OTP
        // await SendOtpAsync(new SendOtpDto { MobileNo = dto.MobileNo, Channel = OtpChannel.Mobile, Purpose = OtpPurpose.MobileVerification }, cancellationtoken);

        return Result.Ok("Registered. Verify email & mobile and wait for admin approval.");
    }

    // ======================================================
    // 2. Verify Email
    // ======================================================
    public async Task<Result> VerifyEmailAsync(VerifyEmailDto dto, CancellationToken cancellationToken)
    {
        var user = await usersRead.GetByEmailAsync(dto.Email, cancellationToken).ConfigureAwait(false);

        if (user is null)
            return Result.Fail("User not found.");

        var otp = await otpRepository.GetActiveAsync(user.Id, dto.Code, OtpPurpose.EmailVerification, cancellationToken).ConfigureAwait(false);

        if (otp is null || !otp.IsActive)
            return Result.Fail("Invalid or expired code.");

        otp.ConsumedAt = DateTime.UtcNow;

        await otpRepository.UpdateAsync(otp, cancellationToken).ConfigureAwait(false);

        await usersWrite.MarkEmailVerifiedAsync(user.Id, cancellationToken).ConfigureAwait(false);

        return Result.Ok("Email verified successfully.");
    }

    // ======================================================
    // 3. SendVerificationMail
    // ======================================================
    public async Task<Result> SendVerificationMailAsync(int userId, CancellationToken cancellationToken)
    {
        var user = await usersRead.GetByIdAsync(userId, cancellationToken).ConfigureAwait(false);

        if (user is null)
            return Result.Fail("User not found.");

        if (user.IsEmailVerified)
            return Result.Fail("Email already verified.");

        var emailVerification = new OtpVerification
        {
            UserId = user.Id,
            Purpose = OtpPurpose.EmailVerification,
            Token = otpGenerator.GenerateSecureToken(),
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_tokenOptions.EmailVerificationExpiryMinutes),

        };

        await otpRepository.AddAsync(emailVerification, cancellationToken).ConfigureAwait(false);

        var baseUrl = appUrlProvider.GetBaseUrl();

        string verificationUrl = $"{baseUrl}api/v{_apiOptions.EmailVerificationVersion}/auth/verify-email?email={Uri.EscapeDataString(user.Email)}&token={emailVerification.Token}";

        var html = emailTemplateRenderer.Render(
            "EmailVerification.html",
            new EmailVerificationModel(user.UserName, new Uri(verificationUrl), (int)TimeSpan.FromMinutes(_tokenOptions.EmailVerificationExpiryMinutes).TotalHours));

        await emailService.SendAsync(user.Email, AuthEmailSubjects.EmailVerification, html, cancellationToken: cancellationToken).ConfigureAwait(false);

        return Result.Ok("Email sent successfully.");
    }

    // ======================================================
    // 4. Update Email
    // ======================================================
    public async Task<Result> UpdateEmailAsync(int userId, UpdateEmailDto dto, CancellationToken cancellationToken)
    {
        if (await usersRead.EmailExistsAsync(dto.NewEmail, cancellationToken).ConfigureAwait(false))
        {
            return Result.Fail("Email already exists.");
        }

        await usersWrite.UpdateEmailAsync(userId, dto.NewEmail, cancellationToken).ConfigureAwait(false);

        // Send new verification code
        var emailSendResult = await SendVerificationMailAsync(userId, cancellationToken).ConfigureAwait(false);

        if (emailSendResult.Success)
        {
            Logger.Information("Verification email sent to {Email}", dto.NewEmail);
            return Result.Ok($"Verification email sent to {dto.NewEmail}");
        }
        else
        {
            Logger.Warning("Failed to send verification email to {Email}", dto.NewEmail);

            return emailSendResult;
        }
    }

    // ======================================================
    // 5. Update Mobile
    // ======================================================
    public async Task<Result> UpdateMobileAsync(int userId, UpdateMobileDto dto, CancellationToken cancellationToken)
    {
        var user = await usersRead.GetByIdAsync(userId, cancellationToken).ConfigureAwait(false);

        if (user is null)
            return Result.Fail("User not found.");

        if (await usersRead.MobileExistsAsync(dto.NewMobile, cancellationToken).ConfigureAwait(false))
            return Result.Fail("Mobile already exists.");

        await usersWrite.UpdateMobileAsync(userId, dto.NewMobile, cancellationToken).ConfigureAwait(false);

        //Send OTP to verify new number
        await SendOtpAsync(userId, new SendOtpDto(OtpPurpose.MobileVerification), cancellationToken).ConfigureAwait(false);

        return Result.Ok("Mobile Number updated successfully.");
    }

    //// ======================================================
    //// 6. SendOtp (email/mobile)
    //// ======================================================
    public async Task<Result> SendOtpAsync(int userId, SendOtpDto dto, CancellationToken cancellationToken)
    {
        var user = await usersRead.GetByIdAsync(userId, cancellationToken).ConfigureAwait(false);

        if (user is null)
            return Result.Fail("User not found.");

        var otp = GenerateOtp(user.Id, _otpOptions.Length, dto.Purpose, _otpOptions.ExpiryMinutes);

        await otpRepository.AddAsync(otp, cancellationToken).ConfigureAwait(false);

        if (_otpOptions.Channel == OtpChannel.Email)
        {
            var html = emailTemplateRenderer.Render(
                "OtpEmail.html",
                new OtpEmailModel(user.UserName, otp.Token, _otpOptions.ExpiryMinutes));

            await emailService.SendAsync(user.Email, AuthEmailSubjects.OtpEmail, html, cancellationToken: cancellationToken).ConfigureAwait(false);
        }
        else if (_otpOptions.Channel == OtpChannel.Mobile && smsSender != null)
        {
            await smsSender.SendSmsAsync(user.MobileNo!, $"Your OTP is {otp.Token}", cancellationToken).ConfigureAwait(false);
        }

        return Result.Ok("OTP sent successfully.");
    }

    // ======================================================
    // 7. VerifyOtp (generic)
    // ======================================================
    public async Task<Result> VerifyOtpAsync(int userId, VerifyOtpDto dto, CancellationToken cancellationToken)
    {
        var user = await usersRead.GetByIdAsync(userId, cancellationToken).ConfigureAwait(false);

        if (user is null)
            return Result.Fail("User not found.");

        var otp = await otpRepository.GetActiveAsync(user.Id, dto.Otp, dto.Purpose, cancellationToken).ConfigureAwait(false);

        if (otp is null || !otp.IsActive)
            return Result.Fail("Invalid or expired OTP.");

        otp.ConsumedAt = DateTime.UtcNow;

        await otpRepository.UpdateAsync(otp, cancellationToken).ConfigureAwait(false);

        // apply effect for specific purpose
        switch (dto.Purpose)
        {
            case OtpPurpose.EmailVerification:
                await usersWrite.MarkEmailVerifiedAsync(user.Id, cancellationToken).ConfigureAwait(false);
                break;

            case OtpPurpose.MobileVerification:
                await usersWrite.MarkMobileVerifiedAsync(user.Id, cancellationToken).ConfigureAwait(false);
                break;

            case OtpPurpose.PasswordReset:
                break;

            default:
                break;
        }
        return Result.Ok("Mobile number verified successfully!");
    }

    // ======================================================
    // 8. Login
    // ======================================================
    public async Task<(string? RefreshToken, Result<LoginResponseDto>)> LoginAsync(LoginDto dto, string ip, CancellationToken cancellationToken)
    {
        var user = await usersRead.GetByUserNameOrEmailAsync(dto.Email, cancellationToken).ConfigureAwait(false);

        if (user is null)
            return (null, Result.Fail<LoginResponseDto>("User not found."));

        var responseDto = new LoginResponseUserDto(user.Id, user.FullName, user.Email, user.IsEmailVerified, user.IsMobileVerified, user.ProfileImageUrl,
        await menuReadService.GetUserMenusAsync(user.Id, cancellationToken).ConfigureAwait(false));

        var loginResponseDto = new LoginResponseDto(responseDto);

        if (!user.IsEmailVerified)
            return (null, Result.Fail("Email must be verified.", loginResponseDto));

        if (!user.IsMobileVerified)
            return (null, Result.Fail("Mobile must be verified.", loginResponseDto));

        if (!user.IsApproved)
            return (null, Result.Fail("User not approved.", loginResponseDto));

        if (user.LockoutEndAt.HasValue && user.LockoutEndAt.Value > DateTime.UtcNow)
            return (null, Result.Fail<LoginResponseDto>($"Account locked, your account will be unlocked at {user.LockoutEndAt}."));

        if (!passwordHasher.Verify(user.PasswordHash, dto.Password))
        {
            await usersWrite.MarkFailedLogin(user.Id, cancellationToken).ConfigureAwait(false);

            return (null, Result.Fail<LoginResponseDto>("Invalid credentials."));
        }

        await usersWrite.MarkSuccessLogin(user.Id, cancellationToken).ConfigureAwait(false);

        if (user.MfaEnabled)
        {
            if (string.IsNullOrEmpty(dto.TotpCode) || !mfaService.ValidateTotp(user.MfaSecret ?? string.Empty, dto.TotpCode))
                return (null, Result.Fail<LoginResponseDto>("MFA required/invalid"));
        }

        var accessToken = jwtTokenGenerator.GenerateToken(user.Id, user.FullName, user.Email, user.Roles);

        var refresh = await refreshTokenService.CreateRefreshTokenAsync(user.Id, ip, (int)new TimeSpan(30, 0, 0, 0, 0, 0).TotalMinutes, cancellationToken).ConfigureAwait(false);

        await loginHistoryService.LoginAsync(user.Id, refresh.Id, true, null, httpContextAccessor.HttpContext, cancellationToken).ConfigureAwait(false);

        loginResponseDto.AccessToken = accessToken;

        return (refresh.Token, Result.Ok("Login successful.", loginResponseDto));
    }

    // ======================================================
    // 9. Refresh
    // ======================================================
    public async Task<(string? RefreshToken, Result<string>)> RefreshAsync(string token, string ip, CancellationToken cancellationToken)
    {
        var refreshToken = await refreshTokenService.GetValidRefreshTokenAsync(token, cancellationToken).ConfigureAwait(false);

        if (refreshToken is null || !refreshToken.IsActive)
            return (null, Result.Fail<string>("Invalid refresh token."));

        var user = await usersRead.GetByIdAsync(refreshToken.UserId, cancellationToken).ConfigureAwait(false);
        if (user is null)
            return (null, Result.Fail<string>("User not found."));

        // rotate token
        refreshToken.RevokedAt = DateTime.UtcNow;
        refreshToken.RevokedByIp = ip;
        refreshToken.ReasonRevoked = "Token rotation";

        var accessToken = jwtTokenGenerator.GenerateToken(user.Id, user.FullName, user.Email, user.Roles);
        var newRefresh = await refreshTokenService.CreateRefreshTokenAsync(user.Id, ip, (int)new TimeSpan(30, 0, 0, 0, 0, 0).TotalMinutes, cancellationToken).ConfigureAwait(false);

        refreshToken.ReplacedByToken = newRefresh.Token;

        await refreshTokens.UpdateAsync(refreshToken, cancellationToken).ConfigureAwait(false);

        await refreshTokens.AddAsync(new RefreshToken
        {
            UserId = user.Id,
            Token = newRefresh.Token,
            CreatedAt = DateTime.UtcNow,
            CreatedByIp = ip,
            ExpiresAt = newRefresh.ExpiresAt,
            IsUsed = false,
        }, cancellationToken).ConfigureAwait(false);

        return (newRefresh.Token, Result.Ok("", accessToken));
    }

    // ======================================================
    // 10. Logout
    // ======================================================
    public async Task<Result> LogoutAsync(string refreshToken, string ip, CancellationToken cancellationToken)
    {
        var rt = await refreshTokens.GetByTokenAsync(refreshToken, cancellationToken).ConfigureAwait(false);
        if (rt is null || rt.IsRevoked)
            return Result.Ok();

        rt.RevokedAt = DateTime.UtcNow;
        rt.RevokedByIp = ip;
        rt.ReasonRevoked = "User logout";

        await refreshTokens.UpdateAsync(rt, cancellationToken).ConfigureAwait(false);

        await loginHistoryService.LogoutAsync(rt.Id, cancellationToken).ConfigureAwait(false);

        return Result.Ok("Logged out.");
    }

    // ======================================================
    // 11. ForgotPassword
    // ======================================================
    public async Task<Result> SendForgotPasswordAsync(ForgotPasswordDto dto, CancellationToken cancellationToken)
    {
        var user = await usersRead.GetByEmailAsync(dto.Email, cancellationToken).ConfigureAwait(false);
        if (user is null)
            return Result.Ok("If account exists, a reset link has been sent.");

        var passwordRest = new OtpVerification
        {
            UserId = user.Id,
            Purpose = OtpPurpose.PasswordReset,
            Token = otpGenerator.GenerateSecureToken(),
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_tokenOptions.PasswordResetExpiryMinutes),
        };

        var baseUrl = appUrlProvider.GetBaseUrl();

        string verificationUrl = $"{baseUrl}api/v{_apiOptions.PasswordResetVerificationVersion}/auth/verify-reset-password?email={Uri.EscapeDataString(user.Email)}&token={passwordRest.Token}";

        var html = emailTemplateRenderer.Render(
            "PasswordReset.html",
            new PasswordResetModel(user.UserName, new Uri(verificationUrl), (int)TimeSpan.FromMinutes(_tokenOptions.PasswordResetExpiryMinutes).TotalMinutes));

        await emailService.SendAsync(user.Email, AuthEmailSubjects.PasswordReset, html, cancellationToken: cancellationToken).ConfigureAwait(false);

        return Result.Ok("If account exists, a reset link has been sent.");
    }

    // ======================================================
    // 12. ResetPassword
    // ======================================================
    public async Task<Result> ResetPasswordAsync(ResetPasswordDto dto, CancellationToken cancellationToken)
    {
        var user = await usersRead.GetByEmailAsync(dto.Email, cancellationToken).ConfigureAwait(false);
        if (user is null)
            return Result.Fail("User not found.");

        var otp = await otpRepository.GetActiveAsync(user.Id, dto.Token, OtpPurpose.PasswordReset, cancellationToken).ConfigureAwait(false);
        if (otp is null || !otp.IsActive)
            return Result.Fail("Invalid or expired reset token.");

        otp.ConsumedAt = DateTime.UtcNow;
        await otpRepository.UpdateAsync(otp, cancellationToken).ConfigureAwait(false);

        var (valid, message) = passwordPolicyService.ValidatePassword(dto.NewPassword);
        if (!valid)
            return Result.Fail(message);

        var newHash = passwordHasher.Hash(dto.NewPassword);

        if (await passwordHistoryService.HasUsedPasswordBeforeAsync(user.Id, newHash, _securityOptions.PasswordHistoryCount, cancellationToken).ConfigureAwait(false))
            return Result.Fail("You cannot reuse any of your last passwords.");

        await usersWrite.SetPasswordHashAsync(user.Id, newHash, cancellationToken).ConfigureAwait(false);

        //Adding password history
        await passwordHistoryService.SaveAsync(user.Id, newHash, cancellationToken).ConfigureAwait(false);

        var html = emailTemplateRenderer.Render(
            "PasswordResetSuccess.html",
            new PasswordResetSuccessModel(user.UserName, DateTimeUtil.Now.ToString("f", Constants.IFormatProvider)));

        await emailService.SendAsync(user.Email, AuthEmailSubjects.PasswordResetSuccess, html, cancellationToken: cancellationToken).ConfigureAwait(false);

        return Result.Ok("Password reset successful.");
    }

    // ======================================================
    // 13. ChangePassword
    // ======================================================
    public async Task<Result> ChangePasswordAsync(int userId, ChangePasswordDto dto, CancellationToken cancellationToken)
    {
        var user = await usersRead.GetByIdAsync(userId, cancellationToken).ConfigureAwait(false);
        if (user is null)
            return Result.Fail("User not found.");

        if (!passwordHasher.Verify(user.PasswordHash, dto.OldPassword))
            return Result.Fail("Incorrect current password.");

        var (valid, message) = passwordPolicyService.ValidatePassword(dto.NewPassword);
        if (!valid)
            return Result.Fail(message);

        var newHash = passwordHasher.Hash(dto.NewPassword);

        if (await passwordHistoryService.HasUsedPasswordBeforeAsync(user.Id, newHash, _securityOptions.PasswordHistoryCount, cancellationToken).ConfigureAwait(false))
            return Result.Fail("You cannot reuse any of your last passwords.");

        await usersWrite.SetPasswordHashAsync(user.Id, newHash, cancellationToken).ConfigureAwait(false);

        //Adding password history
        await passwordHistoryService.SaveAsync(user.Id, newHash, cancellationToken).ConfigureAwait(false);

        var html = emailTemplateRenderer.Render(
            "PasswordChanged.html",
            new PasswordChangedModel(user.UserName, DateTimeUtil.Now.ToString("f", Constants.IFormatProvider)));

        await emailService.SendAsync(user.Email, AuthEmailSubjects.PasswordChanged, html, cancellationToken: cancellationToken).ConfigureAwait(false);

        return Result.Ok("Password Changed successful.");
    }

    public async Task<Result> SendApprovalFollowUpMailAsync(int userId, CancellationToken cancellationToken)
    {
        var user = await usersRead.GetByIdAsync(userId, cancellationToken).ConfigureAwait(false);

        if (user is null)
            return Result.Fail("User not found.");

        if (user.IsApproved)
            return Result.Fail("User already approved.");

        if (!string.IsNullOrEmpty(_adminOptions.NotificationEmail))
        {
            var approveUserUrl = $"{_frontendOptions.BaseUrl}admin/user-management/approve-user";

            var html = emailTemplateRenderer.Render(
            "AdminUserApproval.html",
            new AdminUserApprovalModel(user.UserName, user.FullName, user.Email, new Uri(approveUserUrl)));

            await emailService.SendAsync(_adminOptions.NotificationEmail, AuthEmailSubjects.AdminUserApproval, html, cancellationToken: cancellationToken).ConfigureAwait(false);

            return Result.Ok("Approval follow-up mail sent successfully.");
        }
        else
        {
            return Result.Fail("Failed to send follow-up mail due to Notification email address is not configured.");
        }
    }

    // ======================================================
    // OTP Helper
    // ======================================================
    private OtpVerification GenerateOtp(int userId, int otpLength, OtpPurpose purpose, int expiryMinutes)
    {
        var code = otpGenerator.GenerateNumericOtp(otpLength);

        return new OtpVerification
        {
            UserId = userId,
            Token = code,
            Purpose = purpose,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddMinutes(expiryMinutes)
        };
    }
}
