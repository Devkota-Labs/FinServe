namespace Shared.Application.Interfaces.Services;

public interface IEmailTemplateRenderer
{
    string Render(string templateName, IDictionary<string, object?> model);
}