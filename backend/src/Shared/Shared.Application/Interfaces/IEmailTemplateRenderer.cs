namespace Shared.Application.Interfaces;

public interface IEmailTemplateRenderer
{
    string Render(string templateName, object model);
}
