using Notification.Domain.Enums;

namespace Notification.Domain.Metadata;

public static class NotificationTemplateMap
{
    private static readonly Dictionary<(NotificationType, NotificationChannelType), string>
        EmailTemplates = new()
        {
            {(NotificationType.LoginAlert, NotificationChannelType.Email), "LoginAlert"},
            {(NotificationType.SuspiciousLoginAlert, NotificationChannelType.Email), "SuspiciousLoginAlert"},
            {(NotificationType.EmailVerificationRequested, NotificationChannelType.Email), "EmailVerification"},
            {(NotificationType.EmailChanged, NotificationChannelType.Email), "EmailChanged"},
            {(NotificationType.EmailChangedSecurityAlert, NotificationChannelType.Email), "EmailChangedSecurityAlert"},
            {(NotificationType.EmailVerified, NotificationChannelType.Email), "EmailVerified"},
            {(NotificationType.PasswordChanged, NotificationChannelType.Email), "PasswordChanged"},
            {(NotificationType.PasswordResetRequested, NotificationChannelType.Email), "PasswordReset"},
            {(NotificationType.UserApproved, NotificationChannelType.Email), "UserApproved"},
            {(NotificationType.UserUnlocked, NotificationChannelType.Email), "UserUnlocked"},
            {(NotificationType.PasswordExpiring, NotificationChannelType.Email), "PasswordReminder"},
            {(NotificationType.NewUserAdded, NotificationChannelType.Email), "AdminUserApproval"},
            {(NotificationType.SendOtp, NotificationChannelType.Email), "OtpEmail"},
        };

    public static string? ResolveTemplate(NotificationType type, NotificationChannelType channel)
    {
        return EmailTemplates.TryGetValue((type, channel), out var template)
            ? template
            : null;
    }
}