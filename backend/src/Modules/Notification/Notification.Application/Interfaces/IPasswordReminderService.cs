using Shared.Application.Results;

namespace Notification.Application.Interfaces;

public interface IPasswordReminderService
{
    Task<Result> SendReminderAsync(int userId, DateTime expiryDate, CancellationToken cancellationToken = default);
    Task RunBulkRemindersAsync(CancellationToken cancellationToken = default);
}
