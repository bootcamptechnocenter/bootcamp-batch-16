using System.Net.Http.Headers;
using System.Text.Json;
using WebApi.Modules.Master.Dto.Request;
using WebApi.Modules.Master.Dto.Response;
using WebApi.Shared.Entities;
using WebView.Models;

namespace WebView.Services.Impl
{
    public class MstModelClientService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor) : IMstModelClientService
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly JsonSerializerOptions _jsonOptions = new()
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

        public Task<ResMstModelDto> CreateMstModel(ReqMstModelDto dto)
        {
            var client = CreateClient();

            var payload = JsonSerializer.Serialize(dto);
            var content = new StringContent(payload, System.Text.Encoding.UTF8, "application/json");
            var response = client.PostAsync("master/general/models", content).Result;

            response.EnsureSuccessStatusCode();

            var responseBody = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<ApiClientResponse<ResMstModelDto>>(responseBody, _jsonOptions);

            return Task.FromResult(result?.Data ?? new ResMstModelDto());
        }

        public Task DeleteMstModel(int id)
        {
            var client = CreateClient();
            var response = client.DeleteAsync($"master/general/models/{id}").Result;

            response.EnsureSuccessStatusCode();

            return Task.CompletedTask;
        }

        public Task<ResMstModelDto> GetMstModelById(int id)
        {
            var client = CreateClient();
            var response = client.GetAsync($"master/general/models/{id}").Result;

            response.EnsureSuccessStatusCode();

            var responseBody = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<ApiClientResponse<ResMstModelDto>>(responseBody, _jsonOptions);

            return Task.FromResult(result?.Data ?? new ResMstModelDto());
        }

        public Task<PagedResult<ResMstModelDto>> GetMstModels(ReqBaseParamDto dto)
        {
            var client = CreateClient();

            var url = $"master/general/models?page={dto.Page}&limit={dto.Limit}&search={Uri.EscapeDataString(dto.Search ?? string.Empty)}";
            var response = client.GetAsync(url).Result;

            response.EnsureSuccessStatusCode();

            var responseBody = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<ApiClientResponse<List<ResMstModelDto>>>(responseBody, _jsonOptions);

            return Task.FromResult(new PagedResult<ResMstModelDto>
            {
                Items = result?.Data ?? [],
                Pagination = result?.Pagination ?? new Pagination()
            });
        }

        public Task<ResMstModelDto> UpdateMstModel(int id, ReqMstModelUpdateDto dto)
        {
            var client = CreateClient();

            var payload = JsonSerializer.Serialize(dto);
            var content = new StringContent(payload, System.Text.Encoding.UTF8, "application/json");
            var response = client.PutAsync($"master/general/models/{id}", content).Result;

            response.EnsureSuccessStatusCode();

            var responseBody = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<ApiClientResponse<ResMstModelDto>>(responseBody, _jsonOptions);

            return Task.FromResult(result?.Data ?? new ResMstModelDto());
        }
    }
}