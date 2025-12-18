using Shared.Application.Dtos;
using Users.Application.Dtos.User;

namespace Users.Application.Interfaces.Services;

public interface IUserWriteService
{
    Task<AuthUserDto> CreateUserAsync(CreateUserDto dto, CancellationToken cancellationToken = default);

    Task UpdateEmailAsync(int userId, string newEmail, CancellationToken cancellationToken = default);
    Task UpdateMobileAsync(int userId, string newMobile, CancellationToken cancellationToken = default);

    Task SetPasswordHashAsync(int userId, string newPasswordHash, CancellationToken cancellationToken = default);
    Task MarkEmailVerifiedAsync(int userId, CancellationToken cancellationToken = default);
    Task MarkMobileVerifiedAsync(int userId, CancellationToken cancellationToken = default);
    Task MarkFailedLogin(int userId, CancellationToken cancellationToken = default);
    Task MarkSuccessLogin(int userId, CancellationToken cancellationToken = default);

    Task ApproveUser(int userId, CancellationToken cancellationToken = default);
    Task UnlockUser(int userId, CancellationToken cancellationToken = default);
    Task AssignRoles(int userId, ICollection<int> roleIds, CancellationToken cancellationToken = default);
}
