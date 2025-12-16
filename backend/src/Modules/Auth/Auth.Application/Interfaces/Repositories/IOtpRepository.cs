using Auth.Domain.Entities;
using Shared.Application.Interfaces.Repositories;

namespace Auth.Application.Interfaces.Repositories;

public interface IOtpRepository : IRepository<OtpVerification>
{
    Task<OtpVerification?> GetActiveAsync(int userId, string token, OtpPurpose purpose, CancellationToken cancellationToken = default);
}