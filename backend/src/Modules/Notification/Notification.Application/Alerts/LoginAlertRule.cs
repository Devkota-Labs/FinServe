using Notification.Application.Interfaces;
using Shared.Common;
using Shared.Domain.Enums.Notifications;
using Shared.Domain.Notifications;
using Shared.Security.Contracts;

namespace Notification.Application.Alerts;

internal sealed class LoginAlertRule : IDashboardAlertRule
{
    public Task<IReadOnlyList<NotifyCommand>> EvaluateAsync(ILoginEvent context, CancellationToken cancellationToken = default)
    {
        var commands = new List<NotifyCommand>
        {
            // Always In-App login alert
            new() {
                UserId = context.UserId,
                TemplateKey = NotificationTemplateKey.LoginAlert,
                Model = new
                {
                    context.UserName,
                    context.IpAddress,
                    context.Device,
                    LoginTime = context.LoginTime.ToString("f", Constants.IFormatProvider)
                },
                Category = NotificationCategory.Security,
                Severity = NotificationSeverity.Info,
                ActionType = NotificationActionType.None,
                Channels = [NotificationChannel.InApp]
            }
        };

        // Suspicious → Email + In-App
        if (context.IsSuspicious)
        {
            commands.Add(new NotifyCommand
            {
                UserId = context.UserId,
                TemplateKey = NotificationTemplateKey.SuspiciousLoginAlert,
                Model = new
                {
                    context.UserName,
                    context.IpAddress,
                    context.Device,
                    LoginTime = context.LoginTime.ToString("f", Constants.IFormatProvider)
                },
                Category = NotificationCategory.Security,
                Severity = NotificationSeverity.Critical,
                ActionType = NotificationActionType.ResetPassword,
                Channels =
                [
                NotificationChannel.InApp,
                NotificationChannel.Email
            ]
            });
        }

        return Task.FromResult<IReadOnlyList<NotifyCommand>>(commands);
    }
}