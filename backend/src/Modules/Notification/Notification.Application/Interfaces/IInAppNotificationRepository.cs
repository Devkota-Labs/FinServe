using Notification.Domain.Entities;

namespace Notification.Application.Interfaces;

public interface IInAppNotificationRepository
{
    Task AddAsync(InAppNotification notification, CancellationToken cancellationToken = default);
    public Task<InAppNotification> GetAsync(int notificationId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<InAppNotification>> GetByUserIdAsync(int userId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<int> GetUnreadCountAsync(int userId, CancellationToken cancellationToken = default);
    Task MarkAsReadAsync(int notificationId, CancellationToken cancellationToken = default);
}
