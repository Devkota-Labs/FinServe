using Notification.Application.Alerts;
using Notification.Application.Interfaces;
using Notification.Application.Orchestrator;
using Serilog;
using Shared.Common.Services;
using Shared.Security.Contracts;

namespace Notification.Application.Services;

internal sealed class LoginAlertService(
    ILogger logger,
    LoginAlertRule _rule,
    NotificationOrchestrator _orchestrator,
    INotificationDeduplicationService _dedupe
    )
    : BaseService(logger.ForContext<LoginAlertService>(), null)
{
    public async Task HandleAsync(ILoginEvent context, CancellationToken cancellationToken)
    {
        var commands = await _rule.EvaluateAsync(context, cancellationToken).ConfigureAwait(false);

        foreach (var command in commands)
        {
            if (await _dedupe.ExistsAsync(
                    context.UserId,
                    command.TemplateKey,
                    TimeSpan.FromHours(24), cancellationToken).ConfigureAwait(false))
                continue;

            await _orchestrator.SendAsync(command, cancellationToken).ConfigureAwait(false);
        }
    }
}