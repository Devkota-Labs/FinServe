using Microsoft.Extensions.Options;
using Notification.Application.Interfaces;
using Notification.Application.Orchestrator;
using Serilog;
using Shared.Application.Results;
using Shared.Common.Services;
using Shared.Common.Utils;
using Shared.Domain.Enums.Notifications;
using Shared.Domain.Notifications;
using Shared.Infrastructure.Options;
using Shared.Security.Configurations;
using Users.Application.Interfaces.Services;

namespace Notification.Application.Services;

internal sealed class PasswordReminderService(
    ILogger logger,
    IUserReadService userReadService,
    IOptions<FrontendOptions> frontendOptions,
    IOptions<SecurityOptions> securityOptions,
    INotificationDeduplicationService notificationDeduplicationService,
    NotificationOrchestrator notificationOrchestrator
    )
    : BaseService(logger.ForContext<PasswordReminderService>(), null)
    , IPasswordReminderService
{
    private readonly FrontendOptions _frontendOptions = frontendOptions.Value;
    private readonly SecurityOptions _securityOptions = securityOptions.Value;

    public async Task<Result> SendReminderAsync(int userId, DateTime expiryDate, CancellationToken cancellationToken)
    {
        var user = await userReadService.GetByIdAsync(userId, cancellationToken).ConfigureAwait(false);

        if (user == null)
            return Result.Fail("User not found.");

        var changePasswordUrl = $"{_frontendOptions.BaseUrl}change-password";

        var command = new NotifyCommand
        {
            UserId = userId,
            TemplateKey = NotificationTemplateKey.PasswordExpiring,
            Model = new
            {
                UserName = user.UserName,
                DaysLeft = (int)(DateTimeUtil.Now - expiryDate).TotalDays,
                ChangePasswordUrl = new Uri(changePasswordUrl),
            },
            Category = NotificationCategory.Security,
            Severity = NotificationSeverity.Warning,
            ActionType = NotificationActionType.ResetPassword,

            Channels =
            [
                NotificationChannel.InApp,
                NotificationChannel.Email
            ]
        };

        // Prevent spam (manual endpoint respects same rule)
        if (await notificationDeduplicationService.ExistsAsync(
                userId,
                command.TemplateKey,
                TimeSpan.FromHours(24), cancellationToken).ConfigureAwait(false))
            return Result.Ok("Duplicate notification detected.");

        await notificationOrchestrator.SendAsync(command, cancellationToken).ConfigureAwait(false);

        return Result.Ok($"Password Reminder email sent to {user.Email}");
    }

    /// <summary>
    /// Finds users whose passwords expire in exactly or within configured reminder window (e.g., within next 7 days),
    /// sends email reminders and creates dashboard alerts. Returns count of actions done.
    /// </summary>
    public async Task RunBulkRemindersAsync(CancellationToken cancellationToken)
    {
        var reminderWindow = TimeSpan.FromDays(_securityOptions.PasswordExpiryReminderDays);

        var users = await userReadService.GetUsersWithExpiringPasswordsAsync(reminderWindow, cancellationToken).ConfigureAwait(false);

        foreach (var user in users)
        {
            if (user.PasswordExpiryDate is not null)
            {
                await SendReminderAsync(user.Id, user.PasswordExpiryDate.Value, cancellationToken).ConfigureAwait(false);
            }
        }
    }
}
