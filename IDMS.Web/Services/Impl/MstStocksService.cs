using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using IDMS.Modules.Master.Dto.Request;
using IDMS.Modules.Master.Dto.Response;
using IDMS.Shared.Entities;
using IDMS.Web.Models;

namespace IDMS.Web.Services.Impl
{
    public class MstStocksService : IMstStocksService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public MstStocksService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        private HttpClient CreateHttpClient()
        {
            var client = _httpClientFactory.CreateClient("IDMSApi");
            var token = _httpContextAccessor.HttpContext?.User.FindFirst("AccessToken")?.Value
                ?? _httpContextAccessor.HttpContext?.Session.GetString("Token");
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            return client;
        }

        public async Task<PagedResult<ResMstStocksDto>> GetAllStocks(ReqBaseParamDto dto)
        {
            var client = CreateHttpClient();
            var url = $"api/stocks?page={dto.Page}&limit={dto.Limit}&search={Uri.EscapeDataString(dto.Search ?? string.Empty)}";
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiClientResponse<List<ResMstStocksDto>>>(content, _jsonOptions);

            return new PagedResult<ResMstStocksDto>
            {
                Items = result?.Data ?? new List<ResMstStocksDto>(),
                Pagination = result?.pagination ?? new Pagination()
            };
        }

        public async Task<ResMstStocksDto?> GetStockById(int id)
        {
            var client = CreateHttpClient();
            var response = await client.GetAsync($"api/stocks/{id}");
            if (!response.IsSuccessStatusCode) return null;

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiClientResponse<ResMstStocksDto>>(content, _jsonOptions);
            return result?.Data;
        }

        public async Task<bool> CreateStock(ReqCreateStockDto dto)
        {
            var client = CreateHttpClient();
            var json = JsonSerializer.Serialize(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/stocks/stock", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateStock(int id, ReqUpdateStockDto dto)
        {
            var client = CreateHttpClient();
            var json = JsonSerializer.Serialize(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"api/stocks/stock/{id}", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteStock(int id)
        {
            var client = CreateHttpClient();
            var response = await client.DeleteAsync($"api/stocks/stock/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
