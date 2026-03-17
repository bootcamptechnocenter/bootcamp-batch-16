using System.Net.Http.Headers;
using System.Text.Json;
using IDMS.Modules.Master.Dto.Response;
using IDMS.Web.Models;

namespace IDMS.Web.Services.Impl
{
    public class HomeService : IHomeService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public HomeService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory   = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        private HttpClient CreateClient()
        {
            var client = _httpClientFactory.CreateClient("IDMSApi");
            var token  = _httpContextAccessor.HttpContext?.Session.GetString("Token");
            if (!string.IsNullOrEmpty(token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return client;
        }

        public async Task<ResDashboardDto> GetDashboardData()
        {
            var client   = CreateClient();
            var response = await client.GetAsync("master/general/dashboard");
            if (!response.IsSuccessStatusCode)
                return new ResDashboardDto();

            var content = await response.Content.ReadAsStringAsync();
            var result  = JsonSerializer.Deserialize<ApiClientResponse<ResDashboardDto>>(content, _jsonOptions);
            return result?.Data ?? new ResDashboardDto();
        }
    }
}