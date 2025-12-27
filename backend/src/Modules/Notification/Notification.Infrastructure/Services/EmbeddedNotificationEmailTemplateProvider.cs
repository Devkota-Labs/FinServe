using Serilog;
using Shared.Application.Exceptions;
using Shared.Application.Interfaces.Services;
using Shared.Common.Services;
using System.Reflection;

namespace Notification.Infrastructure.Services;

internal sealed class EmbeddedNotificationEmailTemplateProvider(ILogger logger)
    : BaseService(logger.ForContext<EmbeddedNotificationEmailTemplateProvider>(), null)
    , IEmailBodyTemplateProvider
{
    private const string TemplateRoot = "Notification.Infrastructure.EmailTemplates.";
    private readonly Assembly _assembly = typeof(EmbeddedNotificationEmailTemplateProvider).Assembly;

    public string LoadTemplate(string layoutName)
    {
        var resource = TemplateRoot + layoutName + ".html";

        using var stream = _assembly.GetManifestResourceStream(resource)
            ?? throw new DomainException($"Email template not found: {resource}");

        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}