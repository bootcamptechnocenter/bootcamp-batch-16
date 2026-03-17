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
        private const string ApiKeyHeaderName = "X-Api-Key";
        public ApiKeyMiddleware(RequestDelegate next, IConfiguration config)
        {
            _next = next;
            _config = config;
        }

        public async Task Invoke(HttpContext context)
        {
            if(!context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedApiKey))
            {
                context.Response.StatusCode = 401; 
                await context.Response.WriteAsync("API Key is missing");
                return;
            }

            // var apiKey = _config.GetValue<string>(ApiKeyHeaderName);
            // if(!apiKey.Equals(extractedApiKey))
            // {
            //     context.Response.StatusCode = 401;
            //     await context.Response.WriteAsync("API Key is invalid");
            //     return;
            // }

            var configuredApiKey = _config.GetValue<string>("Security:ApiKey");
            if(string.IsNullOrWhiteSpace(configuredApiKey) || !configuredApiKey.Equals(extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("API Key is invalid");
                return;
            }

            await _next(context);
        }
    }
}