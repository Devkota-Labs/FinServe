using Notification.Application.Orchestrator;
using Serilog;
using Shared.Common.Services;
using Shared.Domain.Enums.Notifications;
using Shared.Domain.Notifications;
using Shared.Security.Contracts;

namespace Notification.Application.Handlers;

internal sealed class UserApprovedNotificationHandler(
    ILogger logger,
    NotificationOrchestrator _orchestrator
    )
    : BaseService(logger.ForContext<UserApprovedNotificationHandler>(), null)
{
    public async Task HandleAsync(IUserApprovedEvent evt, CancellationToken cancellationToken)
    {
        var model = new
        {
            evt.UserName,
            evt.LoginUrl,
        };

        //In-app + Email → NEW email
        await _orchestrator.SendAsync(new NotifyCommand
        {
            UserId = evt.UserId,
            TemplateKey = NotificationTemplateKey.UserApproved,
            Model = model,

            Category = NotificationCategory.Admin,
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
