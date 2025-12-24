namespace Shared.Application.Interfaces.Services;

public interface IEmailTemplateRenderer
{
    string Render(string templateName, object model);
}