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
            PropertyNameCaseInsensitive = true,
        };

        public AuthClientService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private sealed class LoginData
        {
            public string? AccessToken { get; set; }
            public DateTime? ExpiredAt { get; set; }
        }

        public async Task<(bool Success, string Token, DateTime ExpiresAt, string Message)> LoginAsync(string email, string password)
        {
            var client = _httpClientFactory.CreateClient("IDMSApi");

            var payload = JsonSerializer.Serialize(new { email, password });
            var content = new StringContent(payload, System.Text.Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/auth/login", content);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var fail = JsonSerializer.Deserialize<ApiClientResponse<object>>(responseBody, _jsonOptions);
                return (false, string.Empty, DateTime.MinValue, $"Login failed: {fail?.Message ?? "Login failed with status code " + response.StatusCode}");
            }

            var result = JsonSerializer.Deserialize<ApiClientResponse<LoginData>>(responseBody, _jsonOptions);
            if (result?.Data?.AccessToken == null)
            {
                return (false, string.Empty, DateTime.MinValue, "Login failed: Invalid response data.");
            }

            var expiresAt = result.Data.ExpiredAt.HasValue
                ? DateTime.SpecifyKind(result.Data.ExpiredAt.Value, DateTimeKind.Utc)
                : DateTime.UtcNow.AddMinutes(30);

            return (true, result.Data.AccessToken, expiresAt, "Login successful.");
        }


    }
}