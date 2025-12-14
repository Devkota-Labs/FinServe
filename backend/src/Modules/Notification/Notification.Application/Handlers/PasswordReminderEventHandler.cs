using Notification.Application.Events;
using Shared.Application.Interfaces;
using Users.Application.Interfaces.Services;

namespace Notification.Application.Handlers;

public class PasswordReminderEventHandler
{
    private readonly IUserReadService _userReadService;
    private readonly IEmailSender _emailSender;

    public PasswordReminderEventHandler(IUserReadService userReadService, IEmailSender emailSender)
    {
        _userReadService = userReadService;
        _emailSender = emailSender;
    }

    public async Task Handle(PasswordReminderEvent evt)
    {
        var user = await _userReadService.GetByIdAsync(evt.UserId).ConfigureAwait(false);
        if (user == null) return;

        var subject = "Password Expiry Reminder";
        var body = $@"Your password will expire on {evt.ExpiryDate:yyyy-MM-dd}. Please update it.";

        await _emailSender.SendEmailAsync(user.Email, subject, body).ConfigureAwait(false);
    }
}
