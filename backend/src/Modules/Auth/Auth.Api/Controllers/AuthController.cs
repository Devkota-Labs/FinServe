using Asp.Versioning;
using Auth.Application.Dtos;
using Auth.Application.Interfaces.Services;
using Auth.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Notification.Application.Interfaces;
using Serilog;
using Shared.Application.Api;
using Shared.Application.Results;
using Shared.Common.Utils;
using Shared.Infrastructure.Options;

namespace Auth.Api.Controllers;

[ApiVersion("1.0")]
public sealed class AuthController(ILogger logger
    , IAuthService authService
    , IPasswordReminderService passwordReminderService
    , IOptions<FrontendOptions> frontendOption)
    : BaseApiController(logger.ForContext<AuthController>())
{
    private readonly FrontendOptions _frontendOptions = frontendOption.Value;

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDto dto, CancellationToken cancellationToken)
    {
        var (refreshToken, serviceResponse) = await authService.LoginAsync(dto, cancellationToken).ConfigureAwait(false);

        if (serviceResponse.Success && refreshToken is not null)
        {
            //Correct cookie append
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = false, // set to true if using HTTPS
                SameSite = SameSiteMode.None, // required for cross-origin cookies
                Expires = DateTimeUtil.Now.AddDays(30),
                Path = "/" // optional, but ensures cookie is available to the whole app
            };
            // Use refresh.Token here
            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }

        return FromResult(serviceResponse);
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto, CancellationToken cancellationToken)
    {
        var serviceResponse = await authService.RegisterAsync(dto, cancellationToken).ConfigureAwait(false);

        return FromResult(serviceResponse);
    }

    [HttpGet("verify-email")]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyEmail([FromQuery] string email, [FromQuery] string token, CancellationToken cancellationToken)
    {
        var serviceResponse = await authService.VerifyEmailAsync(new VerifyEmailDto(email, token), cancellationToken).ConfigureAwait(false);

        var redirectUrl = serviceResponse.Success
        ? $"{_frontendOptions.BaseUrl}email-verified"
        : $"{_frontendOptions.BaseUrl}email-verification-failed?reason={serviceResponse.Message}";

        return Redirect(redirectUrl);
    }

    [HttpPost("send-verification-email/{userId}")]
    public async Task<IActionResult> SendVerificationMail(int userId, CancellationToken cancellationToken)
    {
        var serviceResponse = await authService.SendVerificationMailAsync(userId, cancellationToken).ConfigureAwait(false);

        return FromResult(serviceResponse);
    }

    [HttpPatch("update-email/{userId}")]
    public async Task<IActionResult> UpdateEmail(int userId, [FromBody] UpdateEmailDto updateEmailDto, CancellationToken cancellationToken)
    {
        var serviceResponse = await authService.UpdateEmailAsync(userId, updateEmailDto, cancellationToken).ConfigureAwait(false);

        return FromResult(serviceResponse);
    }

    [HttpPatch("update-mobile/{userId}")]
    public async Task<IActionResult> UpdateMobile(int userId, [FromBody] UpdateMobileDto updateMobileDto, CancellationToken cancellationToken)
    {
        var serviceResponse = await authService.UpdateMobileAsync(userId, updateMobileDto, cancellationToken).ConfigureAwait(false);

        return FromResult(serviceResponse);
    }

    [HttpPost("send-otp/{userId}")]
    public async Task<IActionResult> SendOtp(int userId, CancellationToken cancellationToken)
    {
        var serviceResponse = await authService.SendOtpAsync(userId, new SendOtpDto(OtpPurpose.MobileVerification), cancellationToken).ConfigureAwait(false);

        return FromResult(serviceResponse);
    }

    [HttpPost("verify-otp/{userId}")]
    public async Task<IActionResult> VerifyOtp(int userId, [FromBody] VerifyOtpDto dto, CancellationToken cancellationToken)
    {
        var serviceResponse = await authService.VerifyOtpAsync(userId, dto, cancellationToken).ConfigureAwait(false);

        return FromResult(serviceResponse);
    }

    [HttpGet("refresh")]
    [Authorize]
    public async Task<IActionResult> Refresh(CancellationToken cancellationToken)
    {
        string? refreshToken = Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(refreshToken))
            return FromResult(Result.Fail("Refresh token missing."));

        var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        var (newRefreshToken, serviceResponse) = await authService.RefreshAsync(refreshToken, ip, cancellationToken).ConfigureAwait(false);

        if (serviceResponse.Success && newRefreshToken is not null)
        {
            Response.Cookies.Append(
            "refreshToken",
            newRefreshToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,       // Use HTTPS
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeUtil.Now.AddDays(30),
                Path = "/api/auth/refresh"
            }
            );
        }

        return FromResult(serviceResponse);
    }

    [HttpGet("logout")]
    [Authorize]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        var token = Request.Cookies["refreshToken"]; // read cookie

        if (string.IsNullOrWhiteSpace(token))
        {
            return FromResult(Result.Fail("Refresh token missing."));
        }

        var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        var serviceResponse = await authService.LogoutAsync(token, ip, cancellationToken).ConfigureAwait(false);

        // delete cookie
        Response.Cookies.Delete("refreshToken", new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Path = "/"
        });

        return FromResult(serviceResponse);
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto, CancellationToken cancellationToken)
    {
        var serviceResponse = await authService.SendForgotPasswordAsync(forgotPasswordDto, cancellationToken).ConfigureAwait(false);

        return FromResult(serviceResponse);
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto, CancellationToken cancellationToken)
    {
        var serviceResponse = await authService.ResetPasswordAsync(resetPasswordDto, cancellationToken).ConfigureAwait(false);

        return FromResult(serviceResponse);
    }

    [HttpPost("change-password/{userId}")]
    [Authorize]
    public async Task<IActionResult> ChangePassword(int userId, [FromBody] ChangePasswordDto changePasswordDto, CancellationToken cancellationToken)
    {
        var serviceResponse = await authService.ChangePasswordAsync(userId, changePasswordDto, cancellationToken).ConfigureAwait(false);

        return FromResult(serviceResponse);
    }

    [HttpPost("send-approval-email/{userId}")]
    public async Task<IActionResult> SendApprovalMail(int userId, CancellationToken cancellationToken)
    {
        var serviceResponse = await authService.SendApprovalMailAsync(userId, cancellationToken).ConfigureAwait(false);

        return FromResult(serviceResponse);
    }

    [ApiVersion("2.0")]
    [HttpPost("Reminder/{userId}")]
    public async Task<IActionResult> ManualReminder(int userId, CancellationToken cancellationToken)
    {
        var expiry = DateTimeUtil.Now.AddDays(7); // This is OK because service recalculates internally.
        await passwordReminderService.SendReminderAsync(userId, expiry, cancellationToken).ConfigureAwait(false);
        return Accepted();
    }
}