using Notification.Application.Orchestrator;
using Serilog;
using Shared.Common.Services;
using Shared.Domain.Enums.Notifications;
using Shared.Domain.Notifications;
using Shared.Security.Contracts;

namespace Notification.Application.Handlers;

internal sealed class PasswordResetRequestedNotificationHandler(
    ILogger logger,
    NotificationOrchestrator _orchestrator
    )
    : BaseService(logger.ForContext<PasswordResetRequestedNotificationHandler>(), null)
{
    public async Task HandleAsync(IPasswordResetRequestedEvent evt, CancellationToken cancellationToken)
    {
        var model = new
        {
            evt.UserName,
            evt.ResetLink,
            evt.ExpiryTimeInMinutes
        };

        //In-app + Email → NEW email
        await _orchestrator.SendAsync(new NotifyCommand
        {
            UserId = evt.UserId,
            TemplateKey = NotificationTemplateKey.PasswordResetRequested,
            Model = model,

            Category = NotificationCategory.Security,
            Severity = NotificationSeverity.Success,
            ActionType = NotificationActionType.None,

            Channels =
            [
                NotificationChannel.Email
            ]
        }, cancellationToken).ConfigureAwait(false);
    }
}
