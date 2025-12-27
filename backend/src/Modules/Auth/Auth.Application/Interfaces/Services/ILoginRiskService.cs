namespace Auth.Application.Interfaces.Services;

public interface ILoginRiskService
{
    Task<bool> IsSuspiciousAsync(int userId, string ip, string userAgent, CancellationToken cancellationToken = default);
}