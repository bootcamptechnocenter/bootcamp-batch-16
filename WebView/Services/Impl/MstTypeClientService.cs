using System.Net.Http.Headers;
using System.Text.Json;
using WebApi.Modules.Master.Dto.Request;
using WebApi.Modules.Master.Dto.Response;
using WebApi.Shared.Entities;
using WebView.Models;

namespace WebView.Services.Impl
{
    public class MstTypeClientService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor) : IMstTypeClientService
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

        public Task<ResMstTypeDto> CreateMstType(ReqMstTypeDto dto)
        {
            var client = CreateClient();

            var payload = JsonSerializer.Serialize(dto);
            var content = new StringContent(payload, System.Text.Encoding.UTF8, "application/json");
            var response = client.PostAsync("master/general/types", content).Result;

            response.EnsureSuccessStatusCode();

            var responseBody = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<ApiClientResponse<ResMstTypeDto>>(responseBody, _jsonOptions);

            return Task.FromResult(result?.Data ?? new ResMstTypeDto());
        }

        public Task DeleteMstType(int id)
        {
            var client = CreateClient();
            var response = client.DeleteAsync($"master/general/types/{id}").Result;

            response.EnsureSuccessStatusCode();

            return Task.CompletedTask;
        }

        public Task<ResMstTypeDto> GetMstTypeById(int id)
        {
            var client = CreateClient();
            var response = client.GetAsync($"master/general/types/{id}").Result;

            response.EnsureSuccessStatusCode();

            var content = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<ApiClientResponse<ResMstTypeDto>>(content, _jsonOptions);

            return Task.FromResult(result?.Data ?? new ResMstTypeDto());
        }

        public Task<PagedResult<ResMstTypeDto>> GetMstTypes(ReqBaseParamDto dto)
        {
            var client = CreateClient();

            var url = $"master/general/types?page={dto.Page}&limit={dto.Limit}&search={Uri.EscapeDataString(dto.Search ?? string.Empty)}";
            var response = client.GetAsync(url).Result;

            response.EnsureSuccessStatusCode();

            var content = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<ApiClientResponse<List<ResMstTypeDto>>>(content, _jsonOptions);

            return Task.FromResult(new PagedResult<ResMstTypeDto>
            {
                Items = result?.Data ?? [],
                Pagination = result?.Pagination ?? new Pagination()
            });
        }

        public Task<ResMstTypeDto> UpdateMstType(int id, ReqMstTypeUpdateDto dto)
        {
            var client = CreateClient();

            var payload = JsonSerializer.Serialize(dto);
            var content = new StringContent(payload, System.Text.Encoding.UTF8, "application/json");
            var response = client.PutAsync($"master/general/types/{id}", content).Result;

            response.EnsureSuccessStatusCode();

            var responseBody = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<ApiClientResponse<ResMstTypeDto>>(responseBody, _jsonOptions);

            return Task.FromResult(result?.Data ?? new ResMstTypeDto());
        }
    }
}