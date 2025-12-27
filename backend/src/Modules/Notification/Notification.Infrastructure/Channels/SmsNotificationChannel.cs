using Notification.Application.Interfaces;
using Notification.Application.Models;
using Notification.Domain.Enums;
using Serilog;
using Shared.Common.Services;

namespace Notification.Infrastructure.Channels;

internal sealed class SmsNotificationChannel
    (
    ILogger logger
    )
    : BaseService(logger.ForContext<SmsNotificationChannel>(), null)
    , INotificationChannel
{
    public NotificationChannelType ChannelType => NotificationChannelType.Sms;

    public Task SendAsync(NotificationMessage message, CancellationToken ct)
    {
        //ToDo Call your existing SMS sender here
        return Task.CompletedTask;
    }
}