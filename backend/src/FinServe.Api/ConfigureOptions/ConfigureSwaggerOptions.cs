using Asp.Versioning.ApiExplorer;
using FinServe.Api.Configurations;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FinServe.Api.ConfigureOptions;

internal sealed class ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider, IOptions<AppConfig> appConfig) : IConfigureOptions<SwaggerGenOptions>
{
    public void Configure(SwaggerGenOptions options)
    {
        // Loop through API versions and create a Swagger document per version
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(
                description.GroupName,
                new OpenApiInfo
                {
                    Title = $"{appConfig.Value.Branding.AppName} API",
                    Version = description.ApiVersion.ToString(),
                    Description = description.IsDeprecated
                        ? "This API version is deprecated."
                        : $"{appConfig.Value.Branding.AppName} REST API"
                }
            );
        }
    }
}