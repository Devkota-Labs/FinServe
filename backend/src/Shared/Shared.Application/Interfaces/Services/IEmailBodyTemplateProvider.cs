namespace Shared.Application.Interfaces.Services;

public interface IEmailBodyTemplateProvider
{
    string LoadTemplate(string templateKey);
}