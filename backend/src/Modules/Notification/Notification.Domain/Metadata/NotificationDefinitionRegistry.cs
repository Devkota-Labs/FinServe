using Notification.Domain.Enums;

namespace Notification.Domain.Metadata;

public static class NotificationDefinitionRegistry
{
    private static readonly Dictionary<NotificationType, NotificationDefinition> _map =
        new()
        {
            [NotificationType.LoginAlert] = new NotificationDefinition
            {
                Type = NotificationType.LoginAlert,
                Category = NotificationCategory.Security,
                Severity = NotificationSeverity.Info,
                ActionType = NotificationActionType.View,
                Channels =
                [
                    NotificationChannelType.InApp
                ]
            },

            [NotificationType.SuspiciousLoginAlert] = new NotificationDefinition
            {
                Type = NotificationType.SuspiciousLoginAlert,
                Category = NotificationCategory.Security,
                Severity = NotificationSeverity.Critical,
                ActionType = NotificationActionType.View,
                Channels =
                [
                    NotificationChannelType.Email,
                    NotificationChannelType.InApp,
                    NotificationChannelType.Sms // escalation allowed
                ]
            },

            [NotificationType.EmailVerificationRequested] = new NotificationDefinition
            {
                Type = NotificationType.EmailVerificationRequested,
                Category = NotificationCategory.Security,
                Severity = NotificationSeverity.Warning,
                ActionType = NotificationActionType.Approve,
                Channels =
                [
                    NotificationChannelType.Email
                ]
            },

            [NotificationType.EmailChanged] = new NotificationDefinition
            {
                Type = NotificationType.EmailChanged,
                Category = NotificationCategory.Account,
                Severity = NotificationSeverity.Info,
                ActionType = NotificationActionType.None,
                Channels =
                [
                    NotificationChannelType.Email,
                    NotificationChannelType.InApp
                ]
            },

            [NotificationType.EmailChangedSecurityAlert] = new NotificationDefinition
            {
                Type = NotificationType.EmailChangedSecurityAlert,
                Category = NotificationCategory.Security,
                Severity = NotificationSeverity.Critical,
                ActionType = NotificationActionType.None,
                Channels =
                [
                    NotificationChannelType.Email
                ]
            },

            [NotificationType.EmailVerified] = new NotificationDefinition
            {
                Type = NotificationType.EmailVerified,
                Category = NotificationCategory.Account,
                Severity = NotificationSeverity.Info,
                ActionType = NotificationActionType.None,
                Channels =
                [
                    NotificationChannelType.Email,
                    NotificationChannelType.InApp
                ]
            },

            [NotificationType.PasswordChanged] = new NotificationDefinition
            {
                Type = NotificationType.PasswordChanged,
                Category = NotificationCategory.Security,
                Severity = NotificationSeverity.Info,
                ActionType = NotificationActionType.None,
                Channels =
                [
                    NotificationChannelType.Email,
                    NotificationChannelType.InApp
                ]
            },

            [NotificationType.PasswordResetRequested] = new NotificationDefinition
            {
                Type = NotificationType.PasswordResetRequested,
                Category = NotificationCategory.Security,
                Severity = NotificationSeverity.Warning,
                ActionType = NotificationActionType.ResetPassword,
                Channels =
                [
                    NotificationChannelType.Email
                ]
            },

            [NotificationType.PasswordResetSuccess] = new NotificationDefinition
            {
                Type = NotificationType.PasswordResetSuccess,
                Category = NotificationCategory.Security,
                Severity = NotificationSeverity.Info,
                ActionType = NotificationActionType.None,
                Channels =
                [
                    NotificationChannelType.Email,
                    NotificationChannelType.InApp
                ]
            },

            [NotificationType.UserApproved] = new NotificationDefinition
            {
                Type = NotificationType.UserApproved,
                Category = NotificationCategory.Admin,
                Severity = NotificationSeverity.Info,
                ActionType = NotificationActionType.View,
                Channels =
                [
                    NotificationChannelType.Email,
                    NotificationChannelType.InApp
                ]
            },

            [NotificationType.UserUnlocked] = new NotificationDefinition
            {
                Type = NotificationType.UserUnlocked,
                Category = NotificationCategory.Security,
                Severity = NotificationSeverity.Info,
                ActionType = NotificationActionType.View,
                Channels =
                [
                    NotificationChannelType.Email,
                    NotificationChannelType.InApp
                ]
            },

            [NotificationType.PasswordExpiring] = new NotificationDefinition
            {
                Type = NotificationType.PasswordExpiring,
                Category = NotificationCategory.Security,
                Severity = NotificationSeverity.Warning,
                ActionType = NotificationActionType.ResetPassword,
                Channels =
                [
                    NotificationChannelType.Email,
                    NotificationChannelType.InApp
                ]
            },

            [NotificationType.NewUserAdded] = new NotificationDefinition
            {
                Type = NotificationType.NewUserAdded,
                Category = NotificationCategory.Admin,
                Severity = NotificationSeverity.Info,
                ActionType = NotificationActionType.View,
                Channels =
                [
                    NotificationChannelType.Email,
                    NotificationChannelType.InApp
                ]
            },

            [NotificationType.SendOtp] = new NotificationDefinition
            {
                Type = NotificationType.SendOtp,
                Category = NotificationCategory.Security,
                Severity = NotificationSeverity.Critical,
                ActionType = NotificationActionType.Approve,
                Channels =
                [
                    NotificationChannelType.Email
                ]
            },
        };

    public static NotificationDefinition Get(NotificationType type)
        => _map[type];
}
