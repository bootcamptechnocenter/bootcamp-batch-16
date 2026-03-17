using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using WebApi.Modules.Master.Dto.Request;
using WebApi.Modules.Master.Dto.Response;
using WebApi.Shared.Entities;
using WebView.Models;

namespace WebView.Services.Impl
{
    public class MstStockClientService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor) : IMstStockClientService
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private HttpClient CreateClient()
        {
            var client = _httpClientFactory.CreateClient("WebApi");
            var token = _httpContextAccessor.HttpContext?.Session.GetString("Token");
            if (!string.IsNullOrEmpty(token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return client;
        }

        public async Task<PagedResult<ResMstStockDto>> GetMstStocks(ReqBaseParamDto dto)
        {
            var client = CreateClient();
            var url = $"master/general/stocks?page={dto.Page}&limit={dto.Limit}&search={Uri.EscapeDataString(dto.Search ?? string.Empty)}";
            var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiClientResponse<List<ResMstStockDto>>>(content, _jsonOptions);

            return new PagedResult<ResMstStockDto>
            {
                Items = result?.Data ?? [],
                Pagination = result?.Pagination ?? new Pagination()
            };
        }

        public Task<ResMstStockDto> GetMstStockById(int id)
        {
            var client = CreateClient();
            var response = client.GetAsync($"master/general/stocks/{id}").Result;

            response.EnsureSuccessStatusCode();

            var content = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<ApiClientResponse<ResMstStockDto>>(content, _jsonOptions);

            return Task.FromResult(result?.Data ?? new ResMstStockDto());
        }

        public Task<ResMstStockDto> CreateMstStock(ReqMstStockDto dto)
        {
            var client = CreateClient();

            var payload = JsonSerializer.Serialize(dto);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = client.PostAsync("master/general/stocks", content).Result;

            response.EnsureSuccessStatusCode();

            var responseBody = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<ApiClientResponse<ResMstStockDto>>(responseBody, _jsonOptions);

            return Task.FromResult(result?.Data ?? new ResMstStockDto());
        }

        public Task<ResMstStockDto> UpdateMstStock(int id, ReqMstStockUpdateDto dto)
        {
            var client = CreateClient();

            var payload = JsonSerializer.Serialize(dto);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = client.PutAsync($"master/general/stocks/{id}", content).Result;

            response.EnsureSuccessStatusCode();

            var responseBody = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<ApiClientResponse<ResMstStockDto>>(responseBody, _jsonOptions);

            return Task.FromResult(result?.Data ?? new ResMstStockDto());
        }

        public Task DeleteMstStock(int id)
        {
            var client = CreateClient();
            var response = client.DeleteAsync($"master/general/stocks/{id}").Result;

            response.EnsureSuccessStatusCode();

            return Task.CompletedTask;
        }
    }
}
