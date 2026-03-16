namespace WebApi.Middleware
{
    public class ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        private readonly RequestDelegate _next = next;
        private readonly IConfiguration _configuration = configuration;
        private const string ApiKeyHeaderName = "X-Api-Key";

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedApiKey))
            {
                context.Items["ErrorMessage"] = "Missing API Key";
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            var configuredApiKey = _configuration["Security:ApiKey"];
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