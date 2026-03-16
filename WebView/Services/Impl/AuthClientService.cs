using System.Text;
using System.Text.Json;
using WebApi.Modules.Master.Dto.Request;
using WebApi.Modules.Master.Dto.Response;
using WebView.Models;

namespace WebView.Services.Impl
{
    public class AuthClientService(IHttpClientFactory httpClientFactory) : IAuthClientService
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public async Task<ResAuthClientDto> LoginAsync(ReqAuthLoginDto dto)
        {
            var client = _httpClientFactory.CreateClient("WebApi");

            var payload = JsonSerializer.Serialize(new { email = dto.Email, password = dto.Password });
            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/auth/login", content);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var fail = JsonSerializer.Deserialize<ApiClientResponse<object>>(responseBody, _jsonOptions);
                return new ResAuthClientDto
                {
                    Success = false,
                    Token = string.Empty,
                    ExpiresAt = DateTime.MinValue,
                    Message = $"Login failed: {fail?.Message ?? "Unknown error"}"
                };
            }

            var result = JsonSerializer.Deserialize<ApiClientResponse<ResAuthClientLoginDto>>(responseBody, _jsonOptions);
            if (result?.Data == null)
            {
                return new ResAuthClientDto
                {
                    Success = false,
                    Token = string.Empty,
                    ExpiresAt = DateTime.MinValue,
                    Message = "Login failed: Invalid response from server"
                };
            }

            var expiresAt = result.Data.ExpiresAt == default
                ? DateTime.UtcNow.AddHours(1)
                : DateTime.SpecifyKind(result.Data.ExpiresAt, DateTimeKind.Utc);

            return new ResAuthClientDto
            {
                Success = true,
                Token = result.Data.Token,
                ExpiresAt = expiresAt,
                Message = "Login successful"
            };
        }
    }
}
