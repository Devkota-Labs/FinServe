using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FinServe.Api.Swagger;

internal sealed class ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider = provider;

    public void Configure(SwaggerGenOptions options)
    {
        // Loop through API versions and create a Swagger document per version
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(
                description.GroupName,
                new OpenApiInfo
                {
                    Title = "FinServe API",
                    Version = description.ApiVersion.ToString(),
                    Description = description.IsDeprecated
                        ? "⚠️ This API version is deprecated."
                        : "FinServe REST API"
                }
            );
        }
    }
}