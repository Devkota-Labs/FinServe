using Notification.Application.Orchestrator;
using Serilog;
using Shared.Common.Services;
using Shared.Domain.Enums.Notifications;
using Shared.Domain.Notifications;
using Shared.Security.Contracts;

namespace Notification.Application.Handlers;

internal sealed class PasswordResetSuccessNotificationHandler(
    ILogger logger,
    NotificationOrchestrator _orchestrator
    )
    : BaseService(logger.ForContext<PasswordResetSuccessNotificationHandler>(), null)
{
    public async Task HandleAsync(IPasswordResetSuccessEvent evt, CancellationToken cancellationToken)
    {
        var model = new
        {
            evt.UserName,
            evt.Timestamp
        };

        //In-app + Email → NEW email
        await _orchestrator.SendAsync(new NotifyCommand
        {
            UserId = evt.UserId,
            TemplateKey = NotificationTemplateKey.PasswordResetSuccess,
            Model = model,

            Category = NotificationCategory.Security,
            Severity = NotificationSeverity.Success,
            ActionType = NotificationActionType.None,

            Channels =
            [
                NotificationChannel.InApp,
                NotificationChannel.Email
            ]
        }, cancellationToken).ConfigureAwait(false);
    }
}
