using Notification.Application.Interfaces;
using Notification.Application.Models;
using Notification.Domain.Enums;
using Serilog;
using Shared.Application.Exceptions;
using Shared.Application.Interfaces.Services;
using Shared.Common.Services;

namespace Notification.Infrastructure.Channels;

internal sealed class EmailNotificationChannel(
    ILogger logger,
    IEmailService emailService
    )
    : BaseService(logger.ForContext<EmailNotificationChannel>(), null)
    , INotificationChannel
{
    public NotificationChannelType ChannelType => NotificationChannelType.Email;

    public async Task SendAsync(NotificationMessage message, CancellationToken cancellationToken = default)
    {
        //HARD FAIL IF EMAIL IS MISSING (DO NOT SILENTLY SKIP)
        if (string.IsNullOrWhiteSpace(message.To))
        {
            Logger.Error("Email address missing for notification {Type} (UserId: {UserId})", message.Type, message.UserId);

            throw new DomainException("Email address is required for EmailNotificationChannel.");
        }

        try
        {
            await emailService.SendAsync(
                to: message.To,
                subject: message.Title,
                htmlBody: message.Body,
                cancellationToken: cancellationToken).ConfigureAwait(false);

            Logger.Information("Email notification sent. Type: {Type}, UserId: {UserId}, Email: {Email}", message.Type, message.UserId, message.To);
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "Failed to send email notification. Type: {Type}, UserId: {UserId}, Email: {Email}", message.Type, message.UserId, message.To);

            // IMPORTANT: rethrow so Outbox marks it Failed
            throw;
        }
    }
}