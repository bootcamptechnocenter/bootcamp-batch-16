using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using IDMS.Common;

namespace IDMS.Middleware
{
    public class RequestMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestMiddleware> _logger;

        public RequestMiddleware(RequestDelegate next, ILogger<RequestMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var reqId = context.TraceIdentifier;
            context.Items["ReqId"] = reqId;

            _logger.LogInformation("Handling request: {Method} {Path} | ReqId: {ReqId}", context.Request.Method, context.Request.Path, reqId);

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred while processing the request. | ReqId: {ReqId}", reqId);

                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                var response = ApiResponse<string>.Failure(ex.Message);
                var json = JsonSerializer.Serialize(response, jsonOptions);

                responseBody.SetLength(0);
                await responseBody.WriteAsync(Encoding.UTF8.GetBytes(json));
            }

            context.Response.Body = originalBodyStream;
            responseBody.Seek(0, SeekOrigin.Begin);

            if (context.Response.StatusCode >= 400 && responseBody.Length == 0)
            {
                string? message = context.Response.StatusCode switch
                {
                    StatusCodes.Status400BadRequest => "Bad Request",
                    StatusCodes.Status401Unauthorized => "Unauthorized",
                    StatusCodes.Status403Forbidden => "Forbidden",
                    StatusCodes.Status404NotFound => "Not Found",
                    StatusCodes.Status500InternalServerError => "Internal Server Error",
                    StatusCodes.Status405MethodNotAllowed => "Method Not Allowed",
                    StatusCodes.Status429TooManyRequests => "Too Many Requests",
                    StatusCodes.Status408RequestTimeout => "Request Timeout",
                    _ => null
                };

                if (message != null)
                {
                    context.Response.ContentType = "application/json";
                    var response = ApiResponse<string>.Failure(message);
                    var json = JsonSerializer.Serialize(response, jsonOptions);
                    await context.Response.WriteAsync(json);
                    return;
                }
            }

            await responseBody.CopyToAsync(originalBodyStream);
        }
    }

    public static class RequestMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestMiddleware>();
        }
    }
}