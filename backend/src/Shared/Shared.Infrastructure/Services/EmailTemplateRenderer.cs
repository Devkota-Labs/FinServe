using Shared.Application.Interfaces.Services;
using System.Reflection;
using System.Text;

namespace Shared.Infrastructure.Services;

internal sealed class EmailTemplateRenderer : IEmailTemplateRenderer
{
    private const string TemplatePath = "Shared.Common.EmailTemplates";
    private static readonly Assembly TemplateAssembly = typeof(Common.AssemblyMarker).Assembly;
    public string Render(string templateName, object model)
    {
        var layout = Load("Layout.html");
        var body = Load(templateName);

        var mergedBody = ReplaceTokens(body, model);
        var finalHtml = layout.Replace("{{Body}}", mergedBody, StringComparison.InvariantCultureIgnoreCase);

        return ReplaceTokens(finalHtml, model);
    }

    private static string Load(string fileName)
    {
        var assembly = TemplateAssembly;
        var resourceName = $"{TemplatePath}.{fileName}";

        using var stream = assembly.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException($"Template not found: {fileName}");

        using var reader = new StreamReader(stream, Encoding.UTF8);
        return reader.ReadToEnd();
    }

    private static string ReplaceTokens(string content, object model)
    {
        foreach (var prop in model.GetType().GetProperties())
        {
            var token = $"{{{{{prop.Name}}}}}";
            var value = prop.GetValue(model)?.ToString() ?? string.Empty;
            content = content.Replace(token, value, StringComparison.InvariantCultureIgnoreCase);
        }
        return content;
    }
}