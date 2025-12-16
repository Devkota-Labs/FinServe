namespace Auth.Application.Interfaces.Services;

public interface IPasswordHistoryService
{
    /// <summary>
    /// Checks if the new password hash matches any recent passwords.
    /// </summary>
    Task<bool> HasUsedPasswordBeforeAsync(int userId, string newPasswordHash, int historyLimit, CancellationToken cancellationToken);

    /// <summary>
    /// Saves the new password hash to history.
    /// </summary>
    Task SaveAsync(int userId, string passwordHash, CancellationToken cancellationToken);
}
