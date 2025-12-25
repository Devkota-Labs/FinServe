using Shared.Application.Interfaces.Services;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Shared.Infrastructure.Services;

internal sealed class NotificationTemplateRenderer : INotificationTemplateRenderer
{
    private static readonly Regex TokenRegex = new(@"\{\{(\w+)\}\}", RegexOptions.Compiled);

    public string Render(string template, object model)
    {
        if (string.IsNullOrWhiteSpace(template))
            return string.Empty;

        if (model == null)
            return template;

        var properties = model
            .GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .ToDictionary(
                p => p.Name,
                p => p.GetValue(model)?.ToString() ?? string.Empty);

        return TokenRegex.Replace(template, match =>
        {
            var key = match.Groups[1].Value;

            return properties.TryGetValue(key, out var value)
                ? value
                : match.Value; // leave unresolved tokens as-is
        });
    }
}