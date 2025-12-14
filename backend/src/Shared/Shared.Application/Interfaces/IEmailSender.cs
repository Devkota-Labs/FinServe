namespace Shared.Application.Interfaces;

public interface IEmailSender
{
    Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default);
}
