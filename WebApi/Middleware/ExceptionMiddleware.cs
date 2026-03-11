using System.Net;
using System.Text.Json;
using WebApi.Common;

namespace WebApi.Middleware
{
    public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<ExceptionMiddleware> _logger = logger;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");

                var message = ex.Message ?? "An error occurred";
                var lower = message.ToLower();
                var statusCode = HttpStatusCode.InternalServerError;

                if (lower.Contains("not found")) statusCode = HttpStatusCode.NotFound;
                else if (lower.Contains("already exists") || lower.Contains("invalid") || lower.Contains("bad request")) statusCode = HttpStatusCode.BadRequest;

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)statusCode;

                var apiResp = ApiResponse<object>.Failure(message);
                var json = JsonSerializer.Serialize(apiResp, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

                await context.Response.WriteAsync(json);
            }
        }
    }
}
