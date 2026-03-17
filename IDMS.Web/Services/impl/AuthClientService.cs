using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using IDMS.Web.Models;

namespace IDMS.Web.Services.Impl
{
    public class AuthClientService : IAuthClientService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public AuthClientService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<(bool Success, string Token, DateTime ExpiresAt, string Message)> LoginAsync(string username,
        string password)
        {
            var client = _httpClientFactory.CreateClient("IDMSApi");

            var payload = JsonSerializer.Serialize(new { email = username, password });
            var content = new StringContent(payload, System.Text.Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/auth/login", content);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var fail = JsonSerializer.Deserialize<ApiClientResponse<object>>(responseBody, _jsonOptions);
                return (false, string.Empty, DateTime.MinValue, $"Login failed: {fail?.Message ?? "Login Failed"}");
            }

            var result = JsonSerializer.Deserialize<ApiClientResponse<LoginData>>(responseBody, _jsonOptions);
            if (result?.Data == null)
            {
                return (false, string.Empty, DateTime.MinValue, "Login failed: Invalid response from server");
            }

            var expiresAt = result.Data.ExpiresAt == default
                ? DateTime.UtcNow.AddHours(1) // Default to 1 hour if not provided
                : DateTime.SpecifyKind(result.Data.ExpiresAt, DateTimeKind.Utc);

            return (true, result.Data.Token, expiresAt, result.Message ?? "Login successful");

        }

        private class LoginData
        {
            public string Token { get; set; } = string.Empty;
            public DateTime ExpiresAt { get; set; }
            public string Email { get; set; } = string.Empty;
        }

    }
}