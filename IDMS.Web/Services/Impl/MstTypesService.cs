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
    public class MstTypesService : IMstTypesService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public MstTypesService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
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

        public async Task<PagedResult<ResMstTypeDto>> GetAllTypes(ReqBaseParamDto dto)
        {
            var client = CreateHttpClient();
            var url = $"api/types/types?page={dto.Page}&limit={dto.Limit}&search={Uri.EscapeDataString(dto.Search ?? string.Empty)}";
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiClientResponse<List<ResMstTypeDto>>>(content, _jsonOptions);

            return new PagedResult<ResMstTypeDto>
            {
                Items = result?.Data ?? new List<ResMstTypeDto>(),
                Pagination = result?.pagination ?? new Pagination()
            };
        }

        public async Task<ResMstTypeDto?> GetTypeById(int id)
        {
            var client = CreateHttpClient();
            var response = await client.GetAsync($"api/types/type/{id}");
            if (!response.IsSuccessStatusCode) return null;

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiClientResponse<ResMstTypeDto>>(content, _jsonOptions);
            return result?.Data;
        }

        public async Task<bool> CreateType(ReqCreateTypeDto dto)
        {
            var client = CreateHttpClient();
            var json = JsonSerializer.Serialize(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/types/type", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateType(int id, ReqUpdateTypeDto dto)
        {
            var client = CreateHttpClient();
            var json = JsonSerializer.Serialize(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"api/types/type/{id}", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteType(int id)
        {
            var client = CreateHttpClient();
            var response = await client.DeleteAsync($"api/types/type/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
