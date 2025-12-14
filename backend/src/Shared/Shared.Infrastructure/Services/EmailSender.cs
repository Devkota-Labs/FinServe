using Microsoft.Extensions.Configuration;
using Serilog;
using Shared.Application.Interfaces;
using Shared.Common.Services;
using System.Net;
using System.Net.Mail;

namespace Shared.Infrastructure.Services;

internal sealed class EmailSender(ILogger logger, IConfiguration configuration) 
    : BaseService(logger.ForContext<EmailSender>(), null), IEmailSender
{
    private readonly IConfiguration _config = configuration;

    public async Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
    {
        var smtp = _config.GetSection("Smtp");
        var host = smtp["Host"];
        var port = int.Parse(smtp["Port"] ?? "587");
        //var user = smtp["User"];
        var pass = smtp["Pass"];
        var from = smtp["From"];

        using var client = new SmtpClient(host, port)
        {
            Credentials = new NetworkCredential(from, pass),
            EnableSsl = true
        };

        var msg = new MailMessage(from, to, subject, body) { IsBodyHtml = true };
        await client.SendMailAsync(msg, cancellationToken).ConfigureAwait(false);
    }
}