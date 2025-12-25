namespace Shared.Application.Interfaces.Services;

public interface INotificationTemplateRenderer
{
    string Render(string template, object model);
}
