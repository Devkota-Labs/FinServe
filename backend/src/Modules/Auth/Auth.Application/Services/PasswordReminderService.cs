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
//        private async Task<BaseResponse> SendReminderEmail(AuthUserDto authUserDto, DateTime expiryDate, CancellationToken cancellationToken)
//        {
//            string subject = "Your password will expire soon";
//            string body = $@"<p>Hello,</p>
//<p>Your password will expire on <b>{expiryDate:dddd, MMMM dd, yyyy}</b>. Please change it before then.</p>
//<p><a href='https://yourapp.example.com/account/change-password'>Click here to change your password</a></p>";


//            await emailSender.SendEmailAsync(user.Email, subject, body, cancellationToken).ConfigureAwait(false);
//        }

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

    /// <summary>
    /// Finds users whose passwords expire in exactly or within configured reminder window (e.g., within next 7 days),
    /// sends email reminders and creates dashboard alerts. Returns count of actions done.
    /// </summary>
    //public async Task<int> RunAsync(CancellationToken cancellationToken = default)
    //{
    //    var reminderDays = configuration.GetValue("Security:PasswordExpiryReminderDays", 7);
    //    var now = DateTime.UtcNow;
    //    var reminderUntil = now.AddDays(reminderDays);

    //    // Find users who have a PasswordExpiryDate set and expiry is between now (inclusive) and reminderUntil (inclusive)
    //    // and who haven't already an identical alert for this expiry (to avoid duplicate emails).
    //    var candidates = await _db.Users
    //        .Where(u => u.IsActive && u.PasswordExpiryDate != null && u.PasswordExpiryDate >= now && u.PasswordExpiryDate <= reminderUntil)
    //        .ToListAsync(cancellationToken).ConfigureAwait(false);

    //    var actions = 0;
    //    foreach (var user in candidates)
    //    {
    //        cancellationToken.ThrowIfCancellationRequested();

    //        // Check if we already created an alert for this expiry for this user in the last (reminderDays + 1) days
    //        var existing = await _db.DashboardAlerts
    //            .Where(a => a.UserId == user.Id && a.Title == "Password Expiry Reminder" && a.CreatedTime >= now.AddDays(-(reminderDays + 1)))
    //            .OrderByDescending(a => a.CreatedTime)
    //            .FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);

    //        if (existing != null)
    //        {
    //            _logger.LogDebug("Skipping user {Email} — existing reminder found.", user.Email);
    //            continue;
    //        }

    //        // Compose message
    //        var daysLeft = (user.PasswordExpiryDate!.Value - now).Days;
    //        if (daysLeft < 0) daysLeft = 0;
    //        var expiryStr = user.PasswordExpiryDate!.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
    //        var title = "Password Expiry Reminder";
    //        var message = $"Hello {user.FullName}, your account password will expire on {expiryStr} (in {daysLeft} day(s)). Please change your password to avoid being locked out.";

    //        // Insert dashboard alert
    //        var alert = new DashboardAlert
    //        {
    //            UserId = user.Id,
    //            Title = title,
    //            Message = message,
    //            CreatedTime = DateTime.UtcNow,
    //            IsRead = false
    //        };
    //        _db.DashboardAlerts.Add(alert);

    //        // Send email (best-effort, do not fail the whole loop)
    //        try
    //        {
    //            string body = $@"
    //<p>Hello <strong>{user.FullName}</strong>,</p>
    //<p>your account password will expire on {expiryStr} (in {daysLeft} day(s)). Please change your password to avoid being locked out.</p>
    //<p>Please click the button below to verify your account:</p>
    //<p><a href='{_config["Frontend:BaseUrl"]}' 
    //      style='padding:10px 20px; background:#4f46e5; color:white; text-decoration:none; border-radius:6px;'>
    //      Login to change
    //   </a>
    //</p>
    //<p>If you didn’t create this account, you can safely ignore this email.</p>
    //";

    //            await _email.SendEmailAsync(user.Email, title, body).ConfigureAwait(false);
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogWarning(ex, "Failed to send password expiry email to {Email}", user.Email);
    //        }

    //        actions++;
    //    }

    //    if (actions > 0)
    //        await _db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

    //    _logger.LogInformation("PasswordExpiryNotificationService completed: {Count} actions", actions);
    //    return actions;
    //}
}
