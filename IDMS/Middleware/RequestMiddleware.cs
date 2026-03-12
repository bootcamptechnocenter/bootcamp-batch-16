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
            context.Items["RequestId"] = reqId;
            _logger.LogInformation("Request {RequestId} started at {Time}", reqId,
            context.Request.Method, context.Request.Path);

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
                _logger.LogError(ex, "Unhandled exception for request {RequestId}", reqId);
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                var response = ApiResponse<string>.Fail(ex.Message);
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
                    StatusCodes.Status401Unauthorized => "Unauthorized",
                    StatusCodes.Status403Forbidden => "Forbidden",
                    StatusCodes.Status404NotFound => "Not Found",
                    StatusCodes.Status405MethodNotAllowed => "Method Not Allowed",
                    StatusCodes.Status400BadRequest => "Bad Request",
                    StatusCodes.Status429TooManyRequests => "Too Many Requests",
                    StatusCodes.Status408RequestTimeout => "Request Timeout",
                    StatusCodes.Status500InternalServerError => "Internal Server Error",
                    _ => null
                };
                
                if (message != null)
                {
                    context.Response.ContentType = "application/json";
                    var response = ApiResponse<string>.Fail(message);
                    var json = JsonSerializer.Serialize(response,jsonOptions);
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