using Auth.Application.Configurations;
using Auth.Application.Interfaces;
using Microsoft.Extensions.Options;
using Serilog;
using Shared.Common.Services;
using System.Net;
using System.Net.Mail;

namespace Auth.Infrastructure.Services;

public sealed class EmailSender : BaseService, IEmailSender
{
    private readonly EmailOptions _emailOptions;
    public EmailSender(ILogger logger, IOptions<EmailOptions> emailOptions)
        : base(logger.ForContext<EmailSender>(), emailOptions.Value)
    {
        _emailOptions = emailOptions.Value;
    }

    public async Task SendEmailAsync(string to, string subject, string html)
    {
        using var client = new SmtpClient(_emailOptions.Host, _emailOptions.Port)
        {
            Credentials = new NetworkCredential(_emailOptions.From, _emailOptions.Pass),
            EnableSsl = true
        };
        var msg = new MailMessage(_emailOptions.From, to, subject, html)
        {
            IsBodyHtml = true,
        };
        await client.SendMailAsync(msg).ConfigureAwait(false);
    }
}
