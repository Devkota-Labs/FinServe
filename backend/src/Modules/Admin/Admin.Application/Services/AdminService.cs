using Admin.Application.Dtos;
using Admin.Application.Interfaces.Services;
using Admin.Application.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog;
using Shared.Application.Dtos;
using Shared.Application.Results;
using Shared.Common.Services;
using Shared.Infrastructure.Background;
using Shared.Infrastructure.Options;
using Shared.Security;
using Users.Application.Interfaces.Services;

namespace Admin.Application.Services;

internal sealed class AdminService(
    ILogger logger,
    IUserReadService userReadService,
    IUserWriteService userWriteService,
    IOptions<FrontendOptions> frontendOption,
    IBackgroundTaskQ backgroundTaskQ
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

    public async Task<Result> ApproveUser(int userId, CancellationToken cancellationToken = default)
    {
        var user = await userReadService.GetByIdAsync(userId, cancellationToken).ConfigureAwait(false);

        if (user is null)
            return Result.Fail("User not found.");

        await userWriteService.ApproveUser(userId, cancellationToken).ConfigureAwait(false);

        var loginUrl = $"{_frontendOptions.BaseUrl}login";

        var evt = new UserApprovedContext
        {
            UserId = user.Id,
            UserName = user.UserName,
            LoginUrl= new Uri(loginUrl),
        };

        //Background-safe execution
        backgroundTaskQ.Queue(async sp =>
        {
            var notifier = sp.GetRequiredService<IUserApprovedNotifier>();

            await notifier.NotifyAsync(evt, cancellationToken).ConfigureAwait(false);
        });

        return Result.Ok("User approved successfully.");
    }

    public async Task<Result> UnlockUser(int userId, CancellationToken cancellationToken = default)
    {
        var user = await userReadService.GetByIdAsync(userId, cancellationToken).ConfigureAwait(false);

        if (user is null)
            return Result.Fail("User not found.");

        await userWriteService.UnlockUser(userId, cancellationToken).ConfigureAwait(false);

        var loginUrl = $"{_frontendOptions.BaseUrl}login";

        var evt = new UserUnlockedContext
        {
            UserId = user.Id,
            UserName = user.UserName,
            LoginUrl = new Uri(loginUrl),
        };

        //Background-safe execution
        backgroundTaskQ.Queue(async sp =>
        {
            var notifier = sp.GetRequiredService<IUserUnlockedNotifier>();

            await notifier.NotifyAsync(evt, cancellationToken).ConfigureAwait(false);
        });

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
