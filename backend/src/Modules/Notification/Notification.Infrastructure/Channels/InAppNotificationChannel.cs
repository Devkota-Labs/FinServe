using Notification.Application.Interfaces;
using Notification.Application.Models;
using Notification.Domain.Entities;
using Notification.Domain.Enums;
using Serilog;
using Shared.Common.Services;

namespace Notification.Infrastructure.Channels;

internal sealed class InAppNotificationChannel(
    ILogger logger,
    IInAppNotificationRepository _repo
    )
    : BaseService(logger.ForContext<InAppNotificationChannel>(), null),
    INotificationChannel
{
    public NotificationChannelType ChannelType => NotificationChannelType.InApp;

    public async Task SendAsync(NotificationMessage message, CancellationToken cancellationToken = default)
    {
        await _repo.AddAsync(new InAppNotification
        {
            UserId = message.UserId,
            Type = message.Type,
            Category = message.Category,
            Severity = message.Severity,
            ActionType = message.ActionType,
            ReferenceType = message.ReferenceType,
            ReferenceId = message.ReferenceId,
            Title = message.Title,
            Body = message.Body,
            ActionUrl = message.ActionUrl
        }, cancellationToken).ConfigureAwait(false);
    }
}