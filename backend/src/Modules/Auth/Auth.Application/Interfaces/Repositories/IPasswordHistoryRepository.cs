using Auth.Domain.Entities;

namespace Auth.Application.Interfaces.Repositories;

//public interface IMobileVerificationTokenRepository : IRepository<MobileVerificationToken>
//{
//    Task<MobileVerificationToken?> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);
//}

public interface IPasswordHistoryRepository
{
    Task<List<PasswordHistory>> GetRecentAsync(int userId, int limit, CancellationToken cancellationToken);
    Task AddAsync(PasswordHistory history, CancellationToken cancellationToken);
}
