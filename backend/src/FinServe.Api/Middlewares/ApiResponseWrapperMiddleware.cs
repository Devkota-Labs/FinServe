//using Shared.Application.Responses;
//using System.Text.Json;

//namespace FinServe.Api.Middlewares;

///// <summary>
///// Wraps responses that are not already ApiResponse into ApiResponse shape automatically.
///// Note: it buffers responses, so use for APIs (not huge binary streams).
///// </summary>
//public sealed class ApiResponseWrapperMiddleware(RequestDelegate next)
//{
//    public async Task Invoke(HttpContext context)
//    {
//        var original = context.Response.Body;

//        using var buffer = new MemoryStream();
//        context.Response.Body = buffer;

//        await next(context).ConfigureAwait(false);

//        buffer.Seek(0, SeekOrigin.Begin);
//        var body = await new StreamReader(buffer).ReadToEndAsync().ConfigureAwait(false);

//        buffer.Seek(0, SeekOrigin.Begin);

//        context.Response.Body = original;

//        if (context.Response.ContentType?.Contains("application/json") == true &&
//            !body.Contains("\"success\":"))
//        {
//            var wrapped = new ApiResponse
//            {
//                Success = context.Response.StatusCode is >= 200 and < 300,
//                Message = "OK",
//                Data = JsonSerializer.Deserialize<object>(body)
//            };

//            await context.Response.WriteAsync(JsonSerializer.Serialize(wrapped)).ConfigureAwait(false);
//        }
//        else
//        {
//            await context.Response.WriteAsync(body).ConfigureAwait(false);
//        }
//    }
//}
