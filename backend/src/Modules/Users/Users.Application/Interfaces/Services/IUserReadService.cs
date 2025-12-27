using Shared.Application.Dtos;
using Users.Application.Dtos.User;

namespace Users.Application.Interfaces.Services;

public interface IUserReadService
{
    Task<AuthUserDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<AuthUserDto?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<AuthUserDto?> GetByMobileAsync(string mobile, CancellationToken cancellationToken = default);
    Task<AuthUserDto?> GetByUserNameOrEmailAsync(string input, CancellationToken cancellationToken = default);

    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> MobileExistsAsync(string mobile, CancellationToken cancellationToken = default);
    Task<bool> UserNameExistsAsync(string username, CancellationToken cancellationToken = default);
    Task<IEnumerable<AuthUserDto>> GetUsersWithExpiringPasswordsAsync(TimeSpan within, CancellationToken cancellationToken = default);

    Task<ICollection<PendingUserDto>> GetUnApprovedUsers(CancellationToken cancellationToken = default);
    Task<ICollection<LockedUserDto>> GetLockedUsers(CancellationToken cancellationToken = default);
    Task<bool> IsUserNameAvailableAsync(string userName, CancellationToken cancellationToken = default);
}
