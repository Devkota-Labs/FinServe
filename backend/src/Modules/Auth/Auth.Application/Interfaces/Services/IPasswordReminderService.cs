using Shared.Application.Results;

namespace Auth.Application.Interfaces.Services;

public interface IPasswordReminderService
{
    Task<Result> SendReminderAsync(int userId, DateTime expiryDate, CancellationToken cancellationToken = default);
    Task RunBulkRemindersAsync(CancellationToken cancellationToken = default);
}
