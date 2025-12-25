namespace Shared.Domain.Notifications;

public static class NotificationTemplateRegistry
{
    private static readonly Dictionary<NotificationTemplateKey, NotificationTemplate>
        _templates = new()
        {
            [NotificationTemplateKey.LoginAlert] = new NotificationTemplate
            {
                Key = NotificationTemplateKey.LoginAlert,
                TitleTemplate = "New Login Detected",
                MessageTemplate = "You logged in on {{LoginTime}} from {{Device}} (IP: {{IpAddress}}).",
                EmailTemplateName = "LoginAlert.html"
            },

            [NotificationTemplateKey.SuspiciousLoginAlert] = new NotificationTemplate
            {
                Key = NotificationTemplateKey.SuspiciousLoginAlert,
                TitleTemplate = "Security Alert: Suspicious Login",
                MessageTemplate = "We detected a login from a new location or device.",
                EmailTemplateName = "SuspiciousLoginAlert.html"
            },

            [NotificationTemplateKey.EmailChanged] = new NotificationTemplate
            {
                Key = NotificationTemplateKey.EmailChanged,
                TitleTemplate = "Email Address Updated",
                MessageTemplate = "Your email address has been changed to {{NewEmail}}.",
                EmailTemplateName = "EmailChanged.html"
            },

            [NotificationTemplateKey.EmailChangedSecurityAlert] = new NotificationTemplate
            {
                Key = NotificationTemplateKey.EmailChangedSecurityAlert,
                TitleTemplate = "Security Alert: Email Changed",
                MessageTemplate = "Your email address was changed from {{OldEmail}} to {{NewEmail}}.",
                EmailTemplateName = "EmailChangedSecurityAlert.html"
            },

            [NotificationTemplateKey.EmailVerified] = new NotificationTemplate
            {
                Key = NotificationTemplateKey.EmailVerified,
                TitleTemplate = "Email Verified Successfully",
                MessageTemplate = "Your email address {{Email}} has been verified successfully.",
                EmailTemplateName = "EmailVerified.html"
            },
            [NotificationTemplateKey.PasswordChanged] = new NotificationTemplate
            {
                Key = NotificationTemplateKey.PasswordChanged,
                TitleTemplate = "Password Changed Successfully",
                MessageTemplate = "Your account password was changed successfully on {{Timestamp}}.",
                EmailTemplateName = "PasswordChanged.html"
            },

            [NotificationTemplateKey.PasswordResetRequested] = new NotificationTemplate
            {
                Key = NotificationTemplateKey.PasswordResetRequested,
                TitleTemplate = "Password Reset Requested",
                MessageTemplate = "A password reset was requested for your account.",
                EmailTemplateName = "PasswordReset.html"
            },

            [NotificationTemplateKey.PasswordResetSuccess] = new NotificationTemplate
            {
                Key = NotificationTemplateKey.PasswordResetSuccess,
                TitleTemplate = "Password Reset Successful",
                MessageTemplate = "Your password was successfully reset on {{Timestamp}}.",
                EmailTemplateName = "PasswordResetSuccess.html"
            },

            [NotificationTemplateKey.UserApproved] = new NotificationTemplate
            {
                Key = NotificationTemplateKey.UserApproved,
                TitleTemplate = "Your account has beed approved",
                MessageTemplate = "Your account has beed approved. You can now access all features.",
                EmailTemplateName = "UserApproved.html"
            },

            [NotificationTemplateKey.UserUnlocked] = new NotificationTemplate
            {
                Key = NotificationTemplateKey.UserUnlocked,
                TitleTemplate = "Your account has beed unlocked",
                MessageTemplate = "Your account has beed unlocked. You can now access all features.",
                EmailTemplateName = "UserUnlocked.html"
            },

            [NotificationTemplateKey.PasswordExpiring] = new NotificationTemplate
            {
                Key = NotificationTemplateKey.PasswordExpiring,
                TitleTemplate = "Password Expiring Soon",
                MessageTemplate = "Your password expires in {{DaysLeft}} days.",
                EmailTemplateName = "PasswordReminder.html"
            },
        };

    public static NotificationTemplate Get(NotificationTemplateKey key)
        => _templates[key];
}
