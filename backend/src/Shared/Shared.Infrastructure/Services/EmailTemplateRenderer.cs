using Microsoft.Extensions.Options;
using Serilog;
using Shared.Application.Exceptions;
using Shared.Application.Interfaces.Services;
using Shared.Common.Services;
using Shared.Common.Utils;
using Shared.Infrastructure.Options;
using System.Reflection;
using System.Text;

namespace Shared.Infrastructure.Services;

internal sealed class EmailTemplateRenderer(
    ILogger logger,
    IEmailLayoutProvider layoutProvider,
    IEnumerable<IEmailBodyTemplateProvider> templateProviders,
    IOptions<BrandingOptions> brandingOptions)
    : BaseService(logger.ForContext<EmailTemplateRenderer>(), null)
    , IEmailTemplateRenderer
{
    // ------------------------------------------------------------
    // Branding merge (ALWAYS applied)
    // ------------------------------------------------------------
    private Dictionary<string, object?> MergeBranding(object model)
    {
        var result = new Dictionary<string, object?>
        {
            ["AppName"] = brandingOptions.Value.AppName,
            ["CompanyName"] = brandingOptions.Value.CompanyName,
            ["SupportEmail"] = brandingOptions.Value.SupportEmail,
            ["AppUrl"] = brandingOptions.Value.AppUrl,
            ["LogoUrl"] = brandingOptions.Value.LogoUrl,
            ["Year"] = DateTimeUtil.Now.Year,
        };

        if (model is IDictionary<string, object?> dict)
        {
            foreach (var kv in dict)
                result[kv.Key] = kv.Value;
        }
        else if (model is not null)
        {
            var props = model.GetType().GetProperties(
                BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in props)
                result[prop.Name] = prop.GetValue(model);
        }

        return result;
    }

    private string LoadBody(string templateKey)
    {
        foreach (var provider in templateProviders)
        {
#pragma warning disable CA1031 // Do not catch general exception types
            try
            {
                return provider.LoadTemplate(templateKey);
            }
            catch
            {
                // try next provider
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }

        throw new DomainException($"Email template not found in any module: {templateKey}");
    }

    public string Render(string templateName, IDictionary<string, object?> model)
    {
        if (string.IsNullOrWhiteSpace(templateName))
            throw new ArgumentException("Template name is required", nameof(templateName));

        // 1 Load layout (Shared Infra)
        var layoutHtml = layoutProvider.LoadLayout("Layout.html");

        // 2️ Load body (first provider that succeeds)
        var bodyHtml = LoadBody(templateName);

        // 3 Merge branding + model
        var mergedModel = MergeBranding(model);

        // 4 Render body
        var renderedBody = ReplaceTokens(bodyHtml, mergedModel);

        // 5 Inject body into layout
        var mergedHtml = layoutHtml.Replace("{{Body}}", renderedBody, StringComparison.InvariantCultureIgnoreCase);

        // 6 Render layout tokens
        return ReplaceTokens(mergedHtml, mergedModel);
    }

    // ------------------------------------------------------------
    // Token replacement (Dictionary + POCO)
    // ------------------------------------------------------------
    private static string ReplaceTokens(string template, object model)
    {
        if (model is null)
            return template;

        var result = new StringBuilder(template);

        //CASE 1: Dictionary<string, object?>
        if (model is IDictionary<string, object?> dict)
        {
            foreach (var kv in dict)
            {
                var token = $"{{{{{kv.Key}}}}}";
                var value = kv.Value?.ToString() ?? string.Empty;
                result.Replace(token, value);
            }

            return result.ToString();
        }

        //CASE 2: POCO object (fallback)
        var properties = model.GetType().GetProperties(
            BindingFlags.Public | BindingFlags.Instance);

        foreach (var prop in properties)
        {
            var token = $"{{{{{prop.Name}}}}}";
            var value = prop.GetValue(model)?.ToString() ?? string.Empty;
            result.Replace(token, value);
        }

        return result.ToString();
    }
}