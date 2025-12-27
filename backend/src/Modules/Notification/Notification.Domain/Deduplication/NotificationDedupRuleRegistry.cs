using Notification.Domain.Enums;

namespace Notification.Domain.Deduplication;

public static class NotificationDedupRuleRegistry
{
    private static readonly Dictionary<NotificationType, NotificationDedupRule> Rules =
        new()
        {
            [NotificationType.LoginAlert] = new NotificationDedupRule
            {
                Enabled = true,
                Window = TimeSpan.FromMinutes(10),
                DedupKeyResolver = data =>
                    data.TryGetValue("DeviceId", out var v) ? v?.ToString() : null
            },

            [NotificationType.SuspiciousLoginAlert] = new NotificationDedupRule
            {
                Enabled = false
            },

            [NotificationType.EmailVerificationRequested] = new NotificationDedupRule
            {
                Enabled = false
            },

            [NotificationType.EmailChanged] = new NotificationDedupRule
            {
                Enabled = false
            },

            [NotificationType.EmailChangedSecurityAlert] = new NotificationDedupRule
            {
                Enabled = false
            },

            [NotificationType.EmailVerified] = new NotificationDedupRule
            {
                Enabled = false
            },

            [NotificationType.PasswordChanged] = new NotificationDedupRule
            {
                Enabled = false
            },

            [NotificationType.PasswordResetRequested] = new NotificationDedupRule
            {
                Enabled = false
            },

            [NotificationType.PasswordResetSuccess] = new NotificationDedupRule
            {
                Enabled = false
            },

            [NotificationType.UserApproved] = new NotificationDedupRule
            {
                Enabled = false
            },

            [NotificationType.UserUnlocked] = new NotificationDedupRule
            {
                Enabled = false
            },

            [NotificationType.PasswordExpiring] = new NotificationDedupRule
            {
                Enabled = true,
                Window = TimeSpan.FromHours(24),
                DedupKeyResolver = _ => null
            },

            [NotificationType.NewUserAdded] = new NotificationDedupRule
            {
                Enabled = false
            },

            [NotificationType.SendOtp] = new NotificationDedupRule
            {
                Enabled = false
            },
        };

    public static NotificationDedupRule Get(NotificationType type)
        => Rules.TryGetValue(type, out var rule)
            ? rule
            : new NotificationDedupRule { Enabled = false };
}
