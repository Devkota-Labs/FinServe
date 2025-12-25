using Admin.Application.Dtos;
using Admin.Application.Interfaces.Services;
using Admin.Application.Models;
using Microsoft.Extensions.Options;
using Serilog;
using Shared.Application.Dtos;
using Shared.Application.Interfaces.Services;
using Shared.Application.Results;
using Shared.Common.Services;
using Shared.Infrastructure.Options;
using Users.Application.Interfaces.Services;

namespace Admin.Application.Services;

internal sealed class AdminService(
    ILogger logger,
    IUserReadService userReadService,
    IUserWriteService userWriteService,
    IEmailService emailService,
    IEmailTemplateRenderer emailTemplateRenderer,
    IOptions<FrontendOptions> frontendOption
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

        var html = emailTemplateRenderer.Render(
        "UserApproved.html",
        new UserApprovedModel(user.UserName, new Uri(loginUrl)));

        await emailService.SendAsync(user.Email, AdminEmailSubjects.UserApproved, html, cancellationToken: cancellationToken).ConfigureAwait(false);

        return Result.Ok("User approved successfully.");
    }

    public async Task<Result> UnlockUser(int userId, CancellationToken cancellationToken = default)
    {
        var user = await userReadService.GetByIdAsync(userId, cancellationToken).ConfigureAwait(false);

        if (user is null)
            return Result.Fail("User not found.");

        await userWriteService.UnlockUser(userId, cancellationToken).ConfigureAwait(false);

        var loginUrl = $"{_frontendOptions.BaseUrl}login";

        var html = emailTemplateRenderer.Render(
        "UserUnlocked.html",
        new UserUnlockedModel(user.UserName, new Uri(loginUrl)));

        await emailService.SendAsync(user.Email, AdminEmailSubjects.UserUnlocked, html, cancellationToken: cancellationToken).ConfigureAwait(false);

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
