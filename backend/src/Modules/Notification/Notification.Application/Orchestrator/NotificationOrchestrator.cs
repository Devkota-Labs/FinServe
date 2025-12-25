using Notification.Application.Interfaces;
using Serilog;
using Shared.Application.Interfaces.Services;
using Shared.Common.Services;
using Shared.Domain.Notifications;

namespace Notification.Application.Orchestrator;

internal sealed class NotificationOrchestrator(
    ILogger logger
    , IInAppNotificationService _inApp
    , IEmailNotificationService _email
    , INotificationTemplateRenderer _renderer)
    : BaseService(logger.ForContext<NotificationOrchestrator>(), null)
{
    private static void Validate(NotifyCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (command.UserId <= 0)
            throw new ArgumentException("Invalid UserId");

        if (command.Channels == null || command.Channels.Count == 0)
            throw new ArgumentException("At least one channel is required");

        if (command.Model == null)
            throw new ArgumentException("Template model cannot be null");
    }

    private async Task DispatchAsync(
        NotificationChannel channel,
        NotifyCommand command,
        NotificationTemplate template,
        string title,
        string message, CancellationToken cancellationToken)
    {
#pragma warning disable CA1031 // Do not catch general exception types
        try
        {
            switch (channel)
            {
                case NotificationChannel.InApp:
                    await _inApp.NotifyAsync(
                        command.UserId,
                        title,
                        message,
                        command.Category,
                        command.Severity,
                        command.ActionType,
                        cancellationToken: cancellationToken)
                        .ConfigureAwait(false);
                    break;

                case NotificationChannel.Email:
                    await _email.SendAsync(
                        command.UserId,
                        template.EmailTemplateName,
                        template.TitleTemplate,
                        command.Model,
                        cancellationToken)
                        .ConfigureAwait(false);
                    break;

                default:
                    throw new NotSupportedException(
                        $"Notification channel '{channel}' is not supported.");
            }
        }
        catch (Exception ex)
        {
            // IMPORTANT: One channel must not break others
            // Log & continue
            Logger.Warning(ex, "Notification channel failed");
        }
#pragma warning restore CA1031 // Do not catch general exception types
    }

    public async Task SendAsync(NotifyCommand command, CancellationToken cancellationToken)
    {
        Validate(command);

        //Resolve template definition
        var template = NotificationTemplateRegistry
            .Get(command.TemplateKey);

        //Render common content (single source of truth)
        var title = _renderer.Render(
            template.TitleTemplate,
            command.Model);

        var message = _renderer.Render(
            template.MessageTemplate,
            command.Model);

        //Deliver per channel (failure isolated)
        foreach (var channel in command.Channels.Distinct())
        {
            await DispatchAsync(
                channel,
                command,
                template,
                title,
                message, cancellationToken).ConfigureAwait(false);
        }
    }
}