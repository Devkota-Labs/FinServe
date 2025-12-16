using Microsoft.Extensions.Options;
using Serilog;
using Shared.Application.Interfaces;
using Shared.Common.Services;
using Shared.Infrastructure.Options;
using System.Net;
using System.Net.Mail;

namespace Shared.Infrastructure.Services;

internal sealed class SmtpEmailService(ILogger logger, IOptions<EmailOptions> options)
    : BaseService(logger.ForContext<SmtpEmailService>(), options.Value)
    , IEmailService
{
    private readonly EmailOptions _options = options.Value;

    public async Task SendAsync(string to, string subject, string htmlBody, string? textBody = null, CancellationToken cancellationToken = default)
    {
        var message = new MailMessage
        {
            From = new MailAddress(_options.FromEmail, _options.FromName),
            Subject = subject,
            Body = htmlBody,
            IsBodyHtml = true
        };

        message.To.Add(to);

        if (!string.IsNullOrWhiteSpace(textBody))
        {
            message.AlternateViews.Add(
                AlternateView.CreateAlternateViewFromString(
                    textBody, null, "text/plain"));
        }

        using var client = new SmtpClient(_options.SmtpHost, _options.SmtpPort)
        {
            Credentials = new NetworkCredential(_options.UserName, _options.Password),
            EnableSsl = true
        };

        await client.SendMailAsync(message, cancellationToken).ConfigureAwait(false);
    }
}