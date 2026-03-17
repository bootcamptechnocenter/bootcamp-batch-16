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
    public class MasterModelService : IMasterModelService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public MasterModelService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        private HttpClient CreateClient()
        {
            var client = _httpClientFactory.CreateClient("IDMSApi");
            var token = _httpContextAccessor.HttpContext?.Session.GetString("Token");
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            return client;
        }

        public async Task<PagedResult<ResMstModelDto>> GetMstModel(ReqBaseParamDto dto)
        {
            var client = CreateClient();
            var url = $"master/general/model?page={dto.Page}&limit={dto.Limit}&search={Uri.EscapeDataString(dto.Search ?? string.Empty)}";
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiClientResponse<List<ResMstModelDto>>>(content, _jsonOptions);

            return new PagedResult<ResMstModelDto>
            {
                Items = result?.Data ?? new List<ResMstModelDto>(),
                Pagination = result?.Pagination ?? new Pagination()
            };
        }

        public async Task<ResMstModelDto?> GetMstModelById(int id)
        {
            var client = CreateClient();
            var response = await client.GetAsync($"master/general/model/{id}");
            if (!response.IsSuccessStatusCode) return null;

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiClientResponse<ResMstModelDto>>(content, _jsonOptions);
            return result?.Data;
        }

        public async Task CreateMstModel(ReqCreateMstModelDto dto)
        {
            var client = CreateClient();
            var json = JsonSerializer.Serialize(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("master/general/model", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task<bool> UpdateMstModel(ReqUpdateMstModelDto dto, int id)
        {
            var client = CreateClient();
            var json = JsonSerializer.Serialize(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"master/general/model/{id}", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteMstModel(int id)
        {
            var client = CreateClient();
            var response = await client.DeleteAsync($"master/general/model/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
