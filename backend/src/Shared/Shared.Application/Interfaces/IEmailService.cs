namespace Shared.Application.Interfaces;

public interface IEmailService
{
    Task SendAsync(string to, string subject, string htmlBody, string? textBody = null, CancellationToken cancellationToken = default);
}
