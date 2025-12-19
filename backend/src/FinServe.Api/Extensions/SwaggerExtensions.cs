using Microsoft.OpenApi.Models;
using Shared.Application.Responses;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FinServe.Api.Extensions;

public static partial class SwaggerExtensions
{
    public static void AddStandardApiResponses(this SwaggerGenOptions c)
    {
        // define reusable response schemas
        //c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "API", Version = "v1" });

        c.MapType<ApiResponse>(() => new OpenApiSchema { Type = "object" });
        c.MapType<PaginatedResponse>(() => new OpenApiSchema { Type = "object" });

        // add global responses for common codes if desired (operation filter better)
    }
}