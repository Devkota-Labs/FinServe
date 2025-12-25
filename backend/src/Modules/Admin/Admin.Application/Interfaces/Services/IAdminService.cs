using Admin.Application.Dtos;
using Shared.Application.Dtos;
using Shared.Application.Results;

namespace Admin.Application.Interfaces.Services;

public interface IAdminService
{
    Task<Result<ICollection<PendingUserDto>>> GetUnApprovedUsersAsync(CancellationToken cancellationToken = default);
    Task<Result> ApproveUser(int userId, CancellationToken cancellationToken = default);
    Task<Result> UnlockUser(int userId, CancellationToken cancellationToken = default);
    Task<Result> AssignRoles(int userId, AssignRoleDto assignRoleDto, CancellationToken cancellationToken = default);
}
