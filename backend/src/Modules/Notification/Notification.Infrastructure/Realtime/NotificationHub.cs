//using Microsoft.AspNetCore.SignalR;
//using Notification.Application.Dtos;

//namespace Notification.Infrastructure.Realtime;

//public class NotificationHub : Hub
//{
//    public override async Task OnConnectedAsync()
//    {
//        var userId = Context.User?.FindFirst("sub")?.Value;

//        if (!string.IsNullOrEmpty(userId))
//        {
//            await Groups.AddToGroupAsync(
//                Context.ConnectionId,
//                $"user-{userId}").ConfigureAwait(false);
//        }

//        await base.OnConnectedAsync().ConfigureAwait(false);
//    }
//}

//public interface INotificationPublisher
//{
//    Task PublishAsync(long userId, NotificationDto notification);
//}

//internal sealed class NotificationPublisher : INotificationPublisher
//{
//    private readonly IHubContext<NotificationHub> _hubContext;

//    public NotificationPublisher(IHubContext<NotificationHub> hubContext)
//    {
//        _hubContext = hubContext;
//    }

//    public async Task PublishAsync(long userId, NotificationDto notification)
//    {
//        await _hubContext.Clients
//            .Group($"user-{userId}")
//            .SendAsync("notificationReceived", notification).ConfigureAwait(false);
//    }
//}
