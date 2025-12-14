using Microsoft.OpenApi.Models;
using Shared.Application.Responses;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FinServe.Api.Extensions;

public static partial class SwaggerExtensions
{
    // OperationFilter example to add standard responses
    public class ApiResponseOperationFilter : IOperationFilter
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