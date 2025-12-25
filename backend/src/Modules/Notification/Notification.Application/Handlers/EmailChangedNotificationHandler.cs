using Notification.Application.Interfaces;
using Notification.Application.Orchestrator;
using Serilog;
using Shared.Common.Services;
using Shared.Domain.Enums.Notifications;
using Shared.Domain.Notifications;
using Shared.Security.Contracts;

namespace Notification.Application.Handlers;

internal sealed class EmailChangedNotificationHandler(
    ILogger logger,
    NotificationOrchestrator _orchestrator,
    IEmailNotificationService _emailService
    )
    : BaseService(logger.ForContext<EmailChangedNotificationHandler>(), null)
{

    public async Task HandleAsync(IEmailChangedEvent evt, CancellationToken cancellationToken)
    {
        var model = new
        {
            evt.UserName,
            evt.OldEmail,
            evt.NewEmail
        };

        //In-app + Email → NEW email
        await _orchestrator.SendAsync(new NotifyCommand
        {
            UserId = evt.UserId,
            TemplateKey = NotificationTemplateKey.EmailChanged,
            Model = model,
            Category = NotificationCategory.Security,
            Severity = NotificationSeverity.Warning,
            ActionType = NotificationActionType.None,
            Channels =
            [
                NotificationChannel.InApp,
                NotificationChannel.Email
            ]
        },cancellationToken).ConfigureAwait(false);

        //Security email → OLD email (email only)
        await _emailService.SendAsync(
            evt.OldEmail,
            "EmailChangedSecurityAlert.html",
            "Your email address has been changed",
            model, cancellationToken).ConfigureAwait(false);
    }
}
