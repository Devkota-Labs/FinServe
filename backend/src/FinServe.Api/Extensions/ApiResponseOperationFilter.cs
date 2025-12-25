using Microsoft.OpenApi.Models;
using Shared.Application.Responses;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FinServe.Api.Extensions;

#pragma warning disable CA1515 // Consider making public types internal
public static partial class SwaggerExtensions
#pragma warning restore CA1515 // Consider making public types internal
{
    // OperationFilter example to add standard responses
#pragma warning disable CA1515 // Consider making public types internal
#pragma warning disable CA1034 // Nested types should not be visible
    public class ApiResponseOperationFilter : IOperationFilter
#pragma warning restore CA1034 // Nested types should not be visible
#pragma warning restore CA1515 // Consider making public types internal
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Responses.TryAdd("400", new OpenApiResponse
            {
                Description = "Bad Request",
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Schema = context.SchemaGenerator.GenerateSchema(typeof(ApiResponse), context.SchemaRepository)
                    }
                }
            });

            operation.Responses.TryAdd("500", new OpenApiResponse
            {
                Description = "Server Error",
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Schema = context.SchemaGenerator.GenerateSchema(typeof(ApiResponse), context.SchemaRepository)
                    }
                }
            });
        }
    }
}