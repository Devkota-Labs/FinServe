using Auth.Application.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Serilog;
using Shared.Application.Interfaces;
using Shared.Application.Results;
using Shared.Common.Services;
using Users.Application.Interfaces.Services;

namespace Auth.Application.Services;

//ToDo EMail Templates and dashboard alerts should be implemented
internal sealed class PasswordReminderService(ILogger logger, IUserReadService userReadService, IEmailSender emailSender, IConfiguration configuration) 
    : BaseService(logger.ForContext<PasswordReminderService>(), null), IPasswordReminderService
{
    public async Task<Result> SendReminderAsync(int userId, DateTime expiryDate, CancellationToken cancellationToken)
    {
        var user = await userReadService.GetByIdAsync(userId, cancellationToken).ConfigureAwait(false);

        if (user == null)
            return Result.Fail("User not found.");

        string subject = "Your password will expire soon";
        string body = $@"<p>Hello,</p>
<p>Your password will expire on <b>{expiryDate:dddd, MMMM dd, yyyy}</b>. Please change it before then.</p>
<p><a href='https://yourapp.example.com/account/change-password'>Click here to change your password</a></p>";


        await emailSender.SendEmailAsync(user.Email, subject, body, cancellationToken).ConfigureAwait(false);

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
