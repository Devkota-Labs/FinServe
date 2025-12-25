namespace Notification.Application.Interfaces;

public interface IEmailNotificationService
{
    Task SendAsync(int userId, string templateName, string subject, object model, CancellationToken cancellationToken);
    Task SendAsync(string email, string templateName, string subject, object model, CancellationToken cancellationToken);
}
