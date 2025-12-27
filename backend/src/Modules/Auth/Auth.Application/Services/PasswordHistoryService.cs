using Auth.Application.Interfaces.Repositories;
using Auth.Application.Interfaces.Services;
using Auth.Domain.Entities;
using Serilog;
using Shared.Common.Services;
using Shared.Common.Utils;
using Shared.Security;

namespace Auth.Application.Services;

internal sealed class PasswordHistoryService(ILogger logger, IPasswordHistoryRepository passwordHistoryRepository, IPasswordHasher passwordHasher)
    : BaseService(logger.ForContext<PasswordHistoryService>(), null), IPasswordHistoryService
{
    private readonly IPasswordHistoryRepository _repo = passwordHistoryRepository;
    private readonly IPasswordHasher _hasher = passwordHasher;

    public async Task<bool> HasUsedPasswordBeforeAsync(int userId, string newPasswordHash, int historyLimit, CancellationToken cancellationToken)
    {
        var recent = await _repo.GetRecentAsync(userId, historyLimit, cancellationToken).ConfigureAwait(false);

        foreach (var old in recent)
        {
            if (_hasher.Verify(old.PasswordHash, newPasswordHash))
                return true;
        }

        return false;
    }

    public async Task SaveAsync(int userId, string passwordHash, CancellationToken cancellationToken)
    {
        var history = new PasswordHistory
        {
            UserId = userId,
            PasswordHash = passwordHash,
            CreatedTime = DateTimeUtil.Now
        };

        await _repo.AddAsync(history, cancellationToken).ConfigureAwait(false);
    }
}
