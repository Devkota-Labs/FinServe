using FinServe.Api.Middlewares;

namespace FinServe.Api.Extensions;

internal static class MiddlewareExtensions
{
    public static IApplicationBuilder UseMiddlewares(this IApplicationBuilder app)
    {
        // global exception handler (must be early in pipeline)
        app.UseMiddleware<DomainExceptionMiddleware>();
        app.UseMiddleware<ExceptionMiddleware>();

        app.UseMiddleware<NullBodyValidationMiddleware>();
        //app.UseMiddleware<ApiResponseWrapperMiddleware>();

        // future custom module middlewares
        return app;
    }
}
