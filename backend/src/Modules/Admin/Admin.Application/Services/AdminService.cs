using Admin.Application.Dtos;
using Admin.Application.Interfaces.Services;
using Microsoft.Extensions.Options;
using Notification.Domain.Enums;
using Notification.Domain.Events;
using Serilog;
using Shared.Application.Dtos;
using Shared.Application.Results;
using Shared.Common.Services;
using Shared.Infrastructure.Background;
using Shared.Infrastructure.Options;
using Users.Application.Dtos.User;
using Users.Application.Interfaces.Services;

namespace Admin.Application.Services;

internal sealed class AdminService(
    ILogger logger,
    IUserReadService userReadService,
    IUserWriteService userWriteService,
    IOptions<FrontendOptions> frontendOption,
    IBackgroundEventQ eventQueue
    )
    : BaseService(logger.ForContext<AdminService>(), null)
    , IAdminService
{
    private readonly FrontendOptions _frontendOptions = frontendOption.Value;

    public async Task<Result<ICollection<PendingUserDto>>> GetUnApprovedUsersAsync(CancellationToken cancellationToken = default)
    {
        var pendingUsers = await userReadService.GetUnApprovedUsers(cancellationToken).ConfigureAwait(false);

        if (pendingUsers == null)
        {
            return Result.Fail<ICollection<PendingUserDto>>("Failed to fetch unapproved users.");
        }

        if (pendingUsers.Count == 0)
        {
            return Result.Ok<ICollection<PendingUserDto>>("No unapproved users found.", null);
        }

        return Result.Ok(pendingUsers);
    }

    public async Task<Result<ICollection<LockedUserDto>>> GetLockedUsersAsync(CancellationToken cancellationToken = default)
    {
        var pendingUsers = await userReadService.GetLockedUsers(cancellationToken).ConfigureAwait(false);

        if (pendingUsers == null)
        {
            return Result.Fail<ICollection<LockedUserDto>>("Failed to fetch locked users.");
        }

        if (pendingUsers.Count == 0)
        {
            return Result.Ok<ICollection<LockedUserDto>>("No locked users found.", null);
        }

        return Result.Ok(pendingUsers);
    }

    public async Task<Result> ApproveUser(int userId, CancellationToken cancellationToken = default)
    {
        var user = await userReadService.GetByIdAsync(userId, cancellationToken).ConfigureAwait(false);

        if (user is null)
            return Result.Fail("User not found.");

        await userWriteService.ApproveUser(userId, cancellationToken).ConfigureAwait(false);

        var loginUrl = $"{_frontendOptions.BaseUrl}auth/login";

        //Raise notification event (AFTER success)
        await eventQueue.EnqueueAsync(
            new NotificationEvent(
                NotificationType.UserApproved,
                user.Id, // notification target user                
                new Dictionary<string, object?>
                {
                    ["UserName"] = user.UserName,
                    ["LoginUrl"] = new Uri(loginUrl)
                }),
            cancellationToken).ConfigureAwait(false);

        return Result.Ok("User approved successfully.");
    }

    public async Task<Result> UnlockUser(int userId, CancellationToken cancellationToken = default)
    {
        var user = await userReadService.GetByIdAsync(userId, cancellationToken).ConfigureAwait(false);

        if (user is null)
            return Result.Fail("User not found.");

        await userWriteService.UnlockUser(userId, cancellationToken).ConfigureAwait(false);

        var loginUrl = $"{_frontendOptions.BaseUrl}auth/login";

        //Publish notification
        await eventQueue.EnqueueAsync(
          new NotificationEvent(
              NotificationType.UserUnlocked,
              user.Id,
              new Dictionary<string, object?>
              {
                  ["UserName"] = user.UserName,
                  ["LoginUrl"] = new Uri(loginUrl),
              }),
          cancellationToken).ConfigureAwait(false);

        return Result.Ok("User unlocked successfully.");
    }

    public async Task<Result> AssignRoles(int userId, AssignRoleDto assignRoleDto, CancellationToken cancellationToken = default)
    {
        var user = await userReadService.GetByIdAsync(userId, cancellationToken).ConfigureAwait(false);

        if (user is null)
            return Result.Fail("User not found.");

        await userWriteService.AssignRoles(userId, assignRoleDto.RoleIds, cancellationToken).ConfigureAwait(false);

        return Result.Ok("Role assigned successfully.");
    }
}
