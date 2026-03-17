using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDMS.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private const string ApiKeyHeaderName = "X-Api-Key";

        public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedApiKey))
            {
                context.Items["ErrorMessage"] = "API Key was not provided";
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            var apiKey = _configuration.GetValue<string>("Security:ApiKey");

            if (string.IsNullOrWhiteSpace(apiKey) || !apiKey.Equals(extractedApiKey))
            {
                context.Items["ErrorMessage"] = "Unauthorized client";
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            await _next(context);
        }

    }
}