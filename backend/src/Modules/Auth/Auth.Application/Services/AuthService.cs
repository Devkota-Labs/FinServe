using Auth.Application.Dtos;
using Auth.Application.Interfaces.Repositories;
using Auth.Application.Interfaces.Services;
using Auth.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Serilog;
using Shared.Application.Interfaces;
using Shared.Application.Results;
using Shared.Common.Services;
using Shared.Common.Utils;
using Shared.Infrastructure.Options;
using Shared.Security;
using Users.Application.Dtos.User;
using Users.Application.Interfaces.Services;

namespace Auth.Application.Services;

internal sealed class AuthService(ILogger logger, 
    IConfiguration configuration, 
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
    )
    : BaseService(logger.ForContext<AuthService>(), null), IAuthService
{
    private readonly TokenOptions _tokenOptions = tokenOptions.Value;
    private readonly OtpOptions _otpOptions = otpOptions.Value;

    // ======================================================
    // 1. Register
    // ======================================================
    public async Task<Result<RegisterResponseDto>> RegisterAsync(RegisterRequestDto dto, CancellationToken cancellationToken)
    {
        // uniqueness checks
        if (await usersRead.EmailExistsAsync(dto.Email, cancellationToken).ConfigureAwait(false))
            return Result<RegisterResponseDto>.Fail("Email already exists.");

        if (await usersRead.MobileExistsAsync(dto.Mobile, cancellationToken).ConfigureAwait(false))
            return Result<RegisterResponseDto>.Fail("Mobile already exists.");

        if (await usersRead.UserNameExistsAsync(dto.UserName, cancellationToken).ConfigureAwait(false))
            return Result<RegisterResponseDto>.Fail("User Name already exists.");

        var (valid, message) = passwordPolicyService.ValidatePassword(dto.Password);
        if (!valid)
            return Result<RegisterResponseDto>.Fail(message);

        var passwordHash = passwordHasher.Hash(dto.Password);

        var createUserDto = new CreateUserDto(dto.UserName, dto.Email, dto.Mobile, dto.Gender, dto.DateOfBirth, dto.FirstName, dto.MiddleName, dto.LastName, dto.CountryId, dto.CityId, dto.StateId, dto.Address, dto.PinCode, passwordHash);

        var authUserDto = await usersWrite.CreateUserAsync(createUserDto, cancellationToken).ConfigureAwait(false);

        //Adding password history
        await passwordHistoryService.SaveAsync(authUserDto.Id, passwordHash, cancellationToken).ConfigureAwait(false);

        // Send email verification
        var emailSendResult = await SendVerificationMailAsync(authUserDto.Id, cancellationToken).ConfigureAwait(false);

        if (emailSendResult.Success)
        {
            Logger.Information("Verification email sent to {Email}", dto.Email);
            //return new ApiResponse<string>(HttpStatusCode.OK, $"Verification email sent to {dto.Email}");
        }
        else
        {
            Logger.Warning("Failed to send verification email to {Email}", dto.Email);

            return Result<RegisterResponseDto>.Fail($"Failed to send verification email to {dto.Email}");
        }

        var adminEmail = configuration["AppConfig:Admin:NotificationEmail"];

        if (!string.IsNullOrEmpty(adminEmail))
        {
            var baseUrl = appUrlProvider.GetBaseUrl();

            var verificationVersion = configuration.GetValue("AppConfig:Api:ApprovedUserVerificationVersion", "1");

            string verificationUrl = $"{baseUrl}api/v{verificationVersion}/auth/verify-email?email={Uri.EscapeDataString(authUserDto.Email)}";

            var html = emailTemplateRenderer.Render(
            "AdminUserApproval.html",
            new
            {
                UserName = authUserDto.FullName,
                UserEmail = authUserDto.Email,
                ApproveLink = verificationUrl,
            });

            await emailService.SendAsync(adminEmail, "New user approval required", html, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        // Optional: send mobile OTP
        // await SendOtpAsync(new SendOtpDto { MobileNo = dto.MobileNo, Channel = OtpChannel.Mobile, Purpose = OtpPurpose.MobileVerification }, cancellationtoken);

        //return new RegisterResponseDto
        //{
        //    UserId = userId,
        //    UserName = dto.UserName,
        //    Email = dto.Email,
        //    MobileNo = dto.MobileNo,
        //    EmailVerificationRequired = true,
        //    MobileVerificationRequired = false
        //};

        var registerResponseDto = new RegisterResponseDto(authUserDto.UserName, dto.Email, dto.Mobile, dto.Gender, dto.DateOfBirth, dto.FirstName, dto.MiddleName, dto.LastName,
            dto.CountryId, dto.CityId, dto.StateId, dto.Address, dto.PinCode);

        return Result<RegisterResponseDto>.Ok("Registered. Verify email & mobile and wait for admin approval.", registerResponseDto);
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

        var emailVerificationVersion = configuration.GetValue("AppConfig:Api:EmailVerificationVersion", "1");

        string verificationUrl = $"{baseUrl}api/v{emailVerificationVersion}/auth/verify-email?email={Uri.EscapeDataString(user.Email)}&token={emailVerification.Token}";

        var html = emailTemplateRenderer.Render(
            "EmailVerification.html",
            new
            {
                AppName = "FinServe",
                Year = DateTime.UtcNow.Year,
                UserName = user.UserName,
                VerificationLink = verificationUrl,
                ExpiryTimeInHours = TimeSpan.FromMinutes(_tokenOptions.EmailVerificationExpiryMinutes).TotalHours,
            });

        await emailService.SendAsync(user.Email, "Verify your email", html, cancellationToken: cancellationToken).ConfigureAwait(false);

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
            return Result<LoginResponseDto>.Fail("User not found.");

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
                new
                {
                    UserName = user.FullName,
                    Otp = otp,
                    ExpiryMinutes = _otpOptions.ExpiryMinutes
                });


            await emailService.SendAsync(user.Email, "Your Verification Code", html, cancellationToken: cancellationToken).ConfigureAwait(false);
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
    public async Task<Result<LoginResponseDto>> LoginAsync(LoginDto dto, string ip, CancellationToken cancellationToken)
    {
        var user = await usersRead.GetByUserNameOrEmailAsync(dto.Email, cancellationToken).ConfigureAwait(false);

        if (user is null)
            return Result<LoginResponseDto>.Fail("User not found.");

        var responseDto = new LoginResponseUserDto(user.Id, user.FullName, user.Email, user.IsEmailVerified, user.IsMobileVerified, user.ProfileImageUrl,
        user.Roles, await menuReadService.GetUserMenusAsync(user.Id, cancellationToken).ConfigureAwait(false));

        var loginResponseDto = new LoginResponseDto(responseDto);

        if (!user.IsEmailVerified)
            return Result<LoginResponseDto>.Fail("Email must be verified.", loginResponseDto);

        if (!user.IsMobileVerified)
            return Result<LoginResponseDto>.Fail("Mobile must be verified.", loginResponseDto);

        if (!user.IsApproved)
            return Result<LoginResponseDto>.Fail("User not approved.", loginResponseDto);

        if (user.LockoutEndAt.HasValue && user.LockoutEndAt.Value > DateTime.UtcNow)
            return Result<LoginResponseDto>.Fail($"Account locked, your account will be unlocked at {user.LockoutEndAt}.");

        if (!passwordHasher.Verify(user.PasswordHash, dto.Password))
        {
            await usersWrite.MarkFailedLogin(user.Id, cancellationToken).ConfigureAwait(false);

            return Result<LoginResponseDto>.Fail("Invalid credentials.");
        }

        await usersWrite.MarkSuccessLogin(user.Id, cancellationToken).ConfigureAwait(false);

        if (user.MfaEnabled)
        {
            if (string.IsNullOrEmpty(dto.TotpCode) || !mfaService.ValidateTotp(user.MfaSecret ?? string.Empty, dto.TotpCode))
                return Result<LoginResponseDto>.Fail("MFA required/invalid");
        }

        var accessToken = jwtTokenGenerator.GenerateToken(user.Id, user.FullName, user.Email, user.Roles);

        var refresh = await refreshTokenService.CreateRefreshTokenAsync(user.Id, ip, (int)new TimeSpan(30, 0, 0, 0, 0, 0).TotalMinutes, cancellationToken).ConfigureAwait(false);

        await loginHistoryService.LoginAsync(user.Id, refresh.Id, true, null, httpContextAccessor.HttpContext, cancellationToken).ConfigureAwait(false);

        loginResponseDto.AccessToken = accessToken;
        loginResponseDto.RefreshToken = refresh.Token;        

        return Result<LoginResponseDto>.Ok("Login successful.", loginResponseDto);
    }

    // ======================================================
    // 9. Refresh
    // ======================================================
    public async Task<Result<LoginResponseDto>> RefreshAsync(string token, string ip, CancellationToken cancellationToken)
    {
        var refreshToken = await refreshTokenService.GetValidRefreshTokenAsync(token, cancellationToken).ConfigureAwait(false);

        if (refreshToken is null || !refreshToken.IsActive)
            return Result<LoginResponseDto>.Fail("Invalid refresh token.");

        var user = await usersRead.GetByIdAsync(refreshToken.UserId, cancellationToken).ConfigureAwait(false);
        if (user is null)
            return Result<LoginResponseDto>.Fail("User not found.");

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


        var responseDto = new LoginResponseUserDto(user.Id, user.FullName, user.Email, user.IsEmailVerified, user.IsMobileVerified, user.ProfileImageUrl,
        user.Roles, await menuReadService.GetUserMenusAsync(user.Id, cancellationToken).ConfigureAwait(false));

        return Result<LoginResponseDto>.Ok("", new LoginResponseDto(responseDto) { AccessToken = accessToken, RefreshToken = newRefresh.Token });
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

        var verificationVersion = configuration.GetValue("AppConfig:Api:PasswordResetVerificationVersion", "1");

        string verificationUrl = $"{baseUrl}api/v{verificationVersion}/auth/verify-reset-password?email={Uri.EscapeDataString(user.Email)}&token={passwordRest.Token}";

        var html = emailTemplateRenderer.Render(
            "PasswordReset.html",
            new
            {
                AppName = "FinServe",
                Year = DateTime.UtcNow.Year,
                UserName = user.UserName,
                ResetLink = verificationUrl,
                ExpiryTimeInMinutes = _tokenOptions.PasswordResetExpiryMinutes,
            });

        await emailService.SendAsync(user.Email, "Reset your password", html, cancellationToken: cancellationToken).ConfigureAwait(false);

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

        var lastN = configuration.GetValue("Security:PasswordHistoryCount", 5);

        if (await passwordHistoryService.HasUsedPasswordBeforeAsync(user.Id, newHash, lastN, cancellationToken).ConfigureAwait(false))
            return Result.Fail("You cannot reuse any of your last passwords.");

        await usersWrite.SetPasswordHashAsync(user.Id, newHash, cancellationToken).ConfigureAwait(false);

        //Adding password history
        await passwordHistoryService.SaveAsync(user.Id, newHash, cancellationToken).ConfigureAwait(false);

        var html = emailTemplateRenderer.Render(
            "PasswordResetSuccess.html",
            new
            {
                UserName = user.UserName,
                Timestamp = DateTimeUtil.Now.ToString("f")
            });

        await emailService.SendAsync(user.Email, "Your password has been reset", html, cancellationToken: cancellationToken).ConfigureAwait(false);

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

        var lastN = configuration.GetValue("Security:PasswordHistoryCount", 5);

        if (await passwordHistoryService.HasUsedPasswordBeforeAsync(user.Id, newHash, lastN, cancellationToken).ConfigureAwait(false))
            return Result.Fail("You cannot reuse any of your last passwords.");

        await usersWrite.SetPasswordHashAsync(user.Id, newHash, cancellationToken).ConfigureAwait(false);

        //Adding password history
        await passwordHistoryService.SaveAsync(user.Id, newHash, cancellationToken).ConfigureAwait(false);

        var html = emailTemplateRenderer.Render(
            "PasswordChanged.html",
            new
            {
                UserName = user.UserName,
                Timestamp = DateTimeUtil.Now.ToString("f")
            });

        await emailService.SendAsync(user.Email, "Your password was changed", html, cancellationToken: cancellationToken).ConfigureAwait(false);

        return Result.Ok("Password Changed successful.");
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
