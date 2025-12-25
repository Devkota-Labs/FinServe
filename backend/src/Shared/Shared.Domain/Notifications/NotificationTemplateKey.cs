namespace Shared.Domain.Notifications;

public enum NotificationTemplateKey
{
    LoginAlert,
    SuspiciousLoginAlert,
    EmailChanged,
    EmailChangedSecurityAlert,
    EmailVerified,
    PasswordChanged,
    PasswordResetRequested,
    PasswordResetSuccess,
    UserApproved,
    UserUnlocked,
    PasswordExpiring,
}
