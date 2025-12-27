using Notification.Application.Interfaces;
using Notification.Application.Models;
using Notification.Domain.Enums;
using Notification.Domain.Events;
using Notification.Domain.Metadata;
using Serilog;
using Shared.Application.Exceptions;
using Shared.Application.Interfaces.Services;
using Shared.Common.Services;
using Users.Application.Interfaces.Services;

namespace Notification.Infrastructure.Rendering;

internal sealed class NotificationTemplateRenderer(
    ILogger logger,
    IEmailTemplateRenderer emailTemplateRenderer,
    IUserReadService userReadService
    )
: BaseService(logger.ForContext<NotificationTemplateRenderer>(), null)
, INotificationTemplateRenderer
{
    // --------------------------------------------------------------------
    // EMAIL
    // --------------------------------------------------------------------
    private NotificationMessage RenderEmail(NotificationEvent notification, NotificationDefinition definition, string emailAddress, Dictionary<string, object?> model)
    {
        var templateName = NotificationTemplateMap.ResolveTemplate(
            notification.Type,
            NotificationChannelType.Email)
            ?? throw new DomainException($"No email template configured for {notification.Type}");

        return new NotificationMessage
        {
            UserId = notification.UserId,
            Type = definition.Type,
            Category = definition.Category,
            Severity = definition.Severity,
            ActionType = definition.ActionType,
            Channel = NotificationChannelType.Email,

            Title = ResolveEmailSubject(notification.Type),
            Body = emailTemplateRenderer.Render(templateName, model),

            To = notification.ToEmail ?? emailAddress
        };
    }

    // --------------------------------------------------------------------
    // IN-APP
    // --------------------------------------------------------------------
    private static NotificationMessage RenderInApp(NotificationEvent notification, NotificationDefinition definition, Dictionary<string, object?> model)
    {
        return new NotificationMessage
        {
            UserId = notification.UserId,
            Type = definition.Type,
            Category = definition.Category,
            Severity = definition.Severity,
            ActionType = definition.ActionType,
            Channel = NotificationChannelType.InApp,

            Title = ResolveInAppTitle(notification.Type),
            Body = ResolveInAppBody(notification.Type, model)
        };
    }

    // --------------------------------------------------------------------
    // SMS
    // --------------------------------------------------------------------
    private static NotificationMessage RenderSms(NotificationEvent notification, NotificationDefinition definition, string mobileNo, Dictionary<string, object?> model)
    {
        return new NotificationMessage
        {
            UserId = notification.UserId,
            Type = definition.Type,
            Category = definition.Category,
            Severity = definition.Severity,
            ActionType = definition.ActionType,
            Channel = NotificationChannelType.Sms,

            Body = ResolveSmsBody(notification.Type, model),
            Phone = mobileNo
        };
    }

    // --------------------------------------------------------------------
    // SUBJECT / BODY RESOLUTION (CENTRALIZED)
    // --------------------------------------------------------------------
    private static string ResolveEmailSubject(NotificationType type) => type switch
    {
        NotificationType.LoginAlert => "New Login Detected",
        NotificationType.SuspiciousLoginAlert => "Security Alert: Suspicious Login",
        NotificationType.EmailVerificationRequested => "Verify your email address",
        NotificationType.EmailChanged => "Email Address Updated",
        NotificationType.EmailChangedSecurityAlert => "Security Alert: Email Changed",
        NotificationType.EmailVerified => "Email Verified Successfully",
        NotificationType.PasswordChanged => "Password Changed Successfully",
        NotificationType.PasswordResetRequested => "Password Reset Requested",
        NotificationType.PasswordResetSuccess => "Password Reset Successful",
        NotificationType.UserApproved => "Your account has beed approved",
        NotificationType.UserUnlocked => "Your account has beed unlocked",
        NotificationType.PasswordExpiring => "Password Expiring Soon",
        NotificationType.NewUserAdded => "New user registered",
        NotificationType.SendOtp => "Mobile verification code",
        _ => "Notification"
    };

    private static string ResolveInAppTitle(NotificationType type) => type switch
    {
        NotificationType.LoginAlert => "New Login Detected",
        NotificationType.SuspiciousLoginAlert => "Security Alert: Suspicious Login",
        NotificationType.EmailVerificationRequested => "Verify your email address",
        NotificationType.EmailChanged => "Email Address Updated",
        NotificationType.EmailChangedSecurityAlert => "Security Alert: Email Changed",
        NotificationType.EmailVerified => "Email Verified Successfully",
        NotificationType.PasswordChanged => "Password Changed Successfully",
        NotificationType.PasswordResetRequested => "Password Reset Requested",
        NotificationType.PasswordResetSuccess => "Password Reset Successful",
        NotificationType.UserApproved => "Your account has beed approved",
        NotificationType.UserUnlocked => "Your account has beed unlocked",
        NotificationType.PasswordExpiring => "Password Expiring Soon",
        NotificationType.NewUserAdded => "New user registered",
        _ => "Notification"
    };

    private static string ResolveInAppBody(NotificationType type, Dictionary<string, object?> model) => type switch
    {
        NotificationType.LoginAlert => $"You logged in on {model.GetValueOrDefault("LoginTime")} from {model.GetValueOrDefault("Device")} (IP: {model.GetValueOrDefault("IpAddress")}).",
        NotificationType.SuspiciousLoginAlert => $"Security Alert: Suspicious Login on {model.GetValueOrDefault("LoginTime")} from {model.GetValueOrDefault("Device")} (IP: {model.GetValueOrDefault("IpAddress")}).",
        NotificationType.EmailVerificationRequested => "Verify your email address",
        NotificationType.EmailChanged => $"Your email address has been changed to {model.GetValueOrDefault("NewEmail")}.",
        NotificationType.EmailChangedSecurityAlert => $"Your email address was changed from {model.GetValueOrDefault("OldEmail")} to {model.GetValueOrDefault("NewEmail")}.",
        NotificationType.EmailVerified => $"Your email address {model.GetValueOrDefault("Email")} has been verified successfully.",
        NotificationType.PasswordChanged => $"Your account password was changed successfully on {model.GetValueOrDefault("Timestamp")}.",
        NotificationType.PasswordResetRequested => "A password reset was requested for your account.",
        NotificationType.PasswordResetSuccess => $"Your password was successfully reset on {model.GetValueOrDefault("Timestamp")}.",
        NotificationType.UserApproved => "Your account has beed approved. You can now access all features.",
        NotificationType.UserUnlocked => "Your account has beed unlocked. You can now access all features.",
        NotificationType.PasswordExpiring => $"Your password expires in {model.GetValueOrDefault("DaysLeft")} days.",
        NotificationType.NewUserAdded => $"New user registered with email {model.GetValueOrDefault("NewEmail")}.",
        _ => "Notification"
    };

    private static string ResolveSmsBody(NotificationType type, Dictionary<string, object?> model) => type switch
    {
        NotificationType.LoginAlert => $"You logged in on {model.GetValueOrDefault("LoginTime")} from {model.GetValueOrDefault("Device")} (IP: {model.GetValueOrDefault("IpAddress")}).",
        NotificationType.SuspiciousLoginAlert => $"Security Alert: Suspicious Login on {model.GetValueOrDefault("LoginTime")} from {model.GetValueOrDefault("Device")} (IP: {model.GetValueOrDefault("IpAddress")}).",
        NotificationType.EmailVerificationRequested => "Verify your email address",
        NotificationType.EmailChanged => $"Your email address has been changed to {model.GetValueOrDefault("NewEmail")}.",
        NotificationType.EmailChangedSecurityAlert => $"Your email address was changed from {model.GetValueOrDefault("OldEmail")} to {model.GetValueOrDefault("NewEmail")}.",
        NotificationType.EmailVerified => $"Your email address {model.GetValueOrDefault("Email")} has been verified successfully.",
        NotificationType.PasswordChanged => $"Your account password was changed successfully on {model.GetValueOrDefault("Timestamp")}.",
        NotificationType.PasswordResetRequested => "A password reset was requested for your account.",
        NotificationType.PasswordResetSuccess => $"Your password was successfully reset on {model.GetValueOrDefault("Timestamp")}.",
        NotificationType.UserApproved => "Your account has beed approved. You can now access all features.",
        NotificationType.UserUnlocked => "Your account has beed unlocked. You can now access all features.",
        NotificationType.PasswordExpiring => $"Your password expires in {model.GetValueOrDefault("DaysLeft")} days.",
        NotificationType.NewUserAdded => $"New user registered with email {model.GetValueOrDefault("NewEmail")}.",
        _ => "Notification"
    };

    public NotificationMessage Render(NotificationEvent notification, NotificationChannelType channel)
    {
        var definition = notification.Definition;

        //Resolve user (single source of contact info)
        var user = userReadService.GetByIdAsync(notification.UserId).Result
            ?? throw new DomainException($"User not found: {notification.UserId}");

        return channel switch
        {
            NotificationChannelType.Email =>
                RenderEmail(notification, definition, user.Email, notification.Data),

            NotificationChannelType.InApp =>
                RenderInApp(notification, definition, notification.Data),

            NotificationChannelType.Sms =>
                RenderSms(notification, definition, user.MobileNo, notification.Data),

            _ => throw new NotSupportedException(
                $"Notification channel not supported: {channel}")
        };
    }
}
