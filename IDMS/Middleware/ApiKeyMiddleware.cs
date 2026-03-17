using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDMS.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _config;
        private const string apiKeyHeaderName = "X-Api-Key";

        public ApiKeyMiddleware(RequestDelegate next, IConfiguration config)
        {
            _next = next;
            _config = config;
        }


        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(apiKeyHeaderName, out var extractedApiKey))
            {
                context.Items["ErrorMessage"] = "Missing API Key";
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }
            var configuredApiKey = _config["Security:ApiKey"];

            if (string.IsNullOrWhiteSpace(configuredApiKey) || !configuredApiKey.Equals(extractedApiKey))
            {
                context.Items["ErrorMessage"] = "Invalid API Key";
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }
            await _next(context);
        }
    }
}