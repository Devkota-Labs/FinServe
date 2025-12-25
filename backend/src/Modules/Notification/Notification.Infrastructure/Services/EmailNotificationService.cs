using Notification.Application.Interfaces;
using Serilog;
using Shared.Application.Interfaces.Services;
using Shared.Common.Services;
using Users.Application.Interfaces.Services;

namespace Notification.Infrastructure.Services;

internal sealed class EmailNotificationService(
    ILogger logger
    , IEmailTemplateRenderer _templateRenderer
    , IUserReadService _userReadService
    , IEmailService _emailSender
    )
    : BaseService(logger.ForContext<EmailNotificationService>(), null)
    , IEmailNotificationService
{
    public async Task SendAsync(int userId, string templateName, string subject, object model, CancellationToken cancellationToken)
    {
        var user = await _userReadService.GetByIdAsync(userId, cancellationToken).ConfigureAwait(false);

        if (user is null)
            return;

        await SendAsync(user.Email, templateName, subject, model, cancellationToken).ConfigureAwait(false);
    }

    public async Task SendAsync(string email, string templateName, string subject, object model, CancellationToken cancellationToken)
    {
        var html = _templateRenderer.Render(templateName, model);

        await _emailSender.SendAsync(email, subject, html, null, cancellationToken).ConfigureAwait(false);
    }
}