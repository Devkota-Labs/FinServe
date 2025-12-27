namespace Notification.Domain.Enums;

public enum NotificationType
{
    LoginAlert,
    SuspiciousLoginAlert,
    EmailVerificationRequested,
    EmailChanged,
    EmailChangedSecurityAlert,
    EmailVerified,
    PasswordChanged,
    PasswordResetRequested,
    PasswordResetSuccess,
    UserApproved,
    UserUnlocked,
    PasswordExpiring,
    NewUserAdded,
    SendOtp,
}
