using Serilog;
using Shared.Application.Exceptions;
using Shared.Application.Interfaces.Services;
using Shared.Common.Services;
using System.Reflection;

namespace Shared.Infrastructure.Services;

internal sealed class EmbeddedEmailLayoutProvider(ILogger logger)
    : BaseService(logger.ForContext<EmbeddedEmailLayoutProvider>(), null)
    , IEmailLayoutProvider
{
    private const string LayoutRoot = "Shared.Infrastructure.EmailTemplates.";
    private readonly Assembly _assembly = typeof(EmbeddedEmailLayoutProvider).Assembly;

    public string LoadLayout(string layoutName)
    {
        var resource = LayoutRoot + layoutName;

        using var stream = _assembly.GetManifestResourceStream(resource)
            ?? throw new DomainException($"Email layout not found: {resource}");

        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
