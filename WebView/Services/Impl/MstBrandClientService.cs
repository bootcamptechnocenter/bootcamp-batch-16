using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using WebApi.Modules.Master.Dto.Request;
using WebApi.Modules.Master.Dto.Response;
using WebApi.Shared.Entities;
using WebView.Models;

namespace WebView.Services.Impl
{
    public class MstBrandClientService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor) : IMstBrandClientService
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

        public async Task<PagedResult<ResMstBrandDto>> GetMstBrands(ReqBaseParamDto dto)
        {
            var client = CreateClient();
            var url = $"master/general/brands?page={dto.Page}&limit={dto.Limit}&search={Uri.EscapeDataString(dto.Search ?? string.Empty)}";
            var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiClientResponse<List<ResMstBrandDto>>>(content, _jsonOptions);

            return new PagedResult<ResMstBrandDto>
            {
                Items = result?.Data ?? [],
                Pagination = result?.Pagination ?? new Pagination()
            };
        }

        public Task<ResMstBrandDto> GetMstBrandById(int id)
        {
            var client = CreateClient();
            var response = client.GetAsync($"master/general/brands/{id}").Result;

            response.EnsureSuccessStatusCode();

            var content = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<ApiClientResponse<ResMstBrandDto>>(content, _jsonOptions);

            return Task.FromResult(result?.Data ?? new ResMstBrandDto());
        }

        public Task<ResMstBrandDto> CreateMstBrand(ReqMstBrandDto dto)
        {
            var client = CreateClient();

            var payload = JsonSerializer.Serialize(dto);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = client.PostAsync("master/general/brands", content).Result;

            response.EnsureSuccessStatusCode();

            var responseBody = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<ApiClientResponse<ResMstBrandDto>>(responseBody, _jsonOptions);

            return Task.FromResult(result?.Data ?? new ResMstBrandDto());
        }

        public Task<ResMstBrandDto> UpdateMstBrand(int id, ReqMstBrandUpdateDto dto)
        {
            var client = CreateClient();

            var payload = JsonSerializer.Serialize(dto);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = client.PutAsync($"master/general/brands/{id}", content).Result;

            response.EnsureSuccessStatusCode();

            var responseBody = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<ApiClientResponse<ResMstBrandDto>>(responseBody, _jsonOptions);

            return Task.FromResult(result?.Data ?? new ResMstBrandDto());
        }

        public Task DeleteMstBrand(int id)
        {
            throw new NotImplementedException();
        }
    }
}
