using Auth.Application.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Serilog;
using Shared.Application.Interfaces;
using Shared.Application.Results;
using Shared.Common.Services;
using Shared.Common.Utils;
using Users.Application.Interfaces.Services;

namespace Auth.Application.Services;

internal sealed class PasswordReminderService(ILogger logger, IUserReadService userReadService, IEmailTemplateRenderer emailTemplateRenderer, IEmailService emailService, IConfiguration configuration) 
    : BaseService(logger.ForContext<PasswordReminderService>(), null), IPasswordReminderService
{
    public async Task<Result> SendReminderAsync(int userId, DateTime expiryDate, CancellationToken cancellationToken)
    {
        var user = await userReadService.GetByIdAsync(userId, cancellationToken).ConfigureAwait(false);

        if (user == null)
            return Result.Fail("User not found.");

        var html = emailTemplateRenderer.Render(
        "PasswordReminder.html",
        new
        {
            UserName = user.FullName,
            DaysLeft = (DateTimeUtil.Now - expiryDate).TotalDays,
            ChangePasswordUrl = "https://app.finserve.com/change-password"
        });

        await emailService.SendAsync(user.Email, "Password expiry reminder", html, cancellationToken: cancellationToken).ConfigureAwait(false);

        return Result.Ok($"Password Reminder email sent to {user.Email}");
    }

    /// <summary>
    /// Finds users whose passwords expire in exactly or within configured reminder window (e.g., within next 7 days),
    /// sends email reminders and creates dashboard alerts. Returns count of actions done.
    /// </summary>
    public async Task RunBulkRemindersAsync(CancellationToken cancellationToken)
    {
        var reminderWindow = TimeSpan.FromDays(configuration.GetValue("Security:PasswordExpiryReminderDays", 7));

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
