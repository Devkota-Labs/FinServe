using Shared.Application.Interfaces.Services;
using Shared.Infrastructure.Options;
using System.Reflection;
using System.Text;

namespace Shared.Infrastructure.Services;

internal sealed class EmailTemplateRenderer(IEmailTemplateContextProvider contextProvider) : IEmailTemplateRenderer
{
    private const string TemplatePath = "Shared.Infrastructure.EmailTemplates";
    private static readonly Assembly TemplateAssembly = Assembly.GetExecutingAssembly();

    private static Dictionary<string, object?> MergeModels(object specificModel, EmailTemplateCommonContext commonContext)
    {
        var result = new Dictionary<string, object?>();

        foreach (var prop in commonContext.GetType().GetProperties())
            result[prop.Name] = prop.GetValue(commonContext);

        foreach (var prop in specificModel.GetType().GetProperties())
            result[prop.Name] = prop.GetValue(specificModel);

        return result;
    }

    public string Render(string templateName, object model)
    {
        var layout = Load("Layout.html");
        var body = Load(templateName);

        // Merge model + common context
        var mergedModel = MergeModels(
            model,
            contextProvider.GetCommonContext());

        var mergedBody = ReplaceTokens(body, mergedModel);
        var finalHtml = layout.Replace("{{Body}}", mergedBody, StringComparison.InvariantCultureIgnoreCase);

        return ReplaceTokens(finalHtml, mergedModel);
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
    private static string ReplaceTokens(string template, IDictionary<string, object?> model)
    {
        foreach (var (key, value) in model)
        {
            template = template.Replace(
                $"{{{{{key}}}}}",
                value?.ToString() ?? string.Empty, StringComparison.InvariantCultureIgnoreCase);
        }

        return template;
    }
}