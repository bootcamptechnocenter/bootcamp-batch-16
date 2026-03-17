using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using IDMS.Modules.Master.Dto.Request;
using IDMS.Modules.Master.Dto.Response;
using IDMS.Shared.Entities;
using IDMS.Web.Models;

namespace IDMS.Web.Services.Impl
{
    public class MstTypeClientService : IMasterTypeService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public MstTypeClientService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
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

        public async Task<PagedResult<ResMstTypeDto>> GetMstType(ReqBaseParamDto dto)
        {
            var client = CreateClient();
            var url = $"master/general/type?page={dto.Page}&limit={dto.Limit}&search={Uri.EscapeDataString(dto.Search ?? string.Empty)}";
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiClientResponse<List<ResMstTypeDto>>>(content, _jsonOptions);

            return new PagedResult<ResMstTypeDto>
            {
                Items = result?.Data ?? new List<ResMstTypeDto>(),
                Pagination = result?.Pagination ?? new Pagination()
            };
        }

        public async Task<ResMstTypeDto?> GetMstTypeById(int id)
        {
            var client = CreateClient();
            var response = await client.GetAsync($"master/general/type/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiClientResponse<ResMstTypeDto>>(content, _jsonOptions);
            return result?.Data;
        }

        public async Task CreateMstType(ReqCreateMstTypeDto dto)
        {
            var client = CreateClient();
            var json = JsonSerializer.Serialize(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("master/general/type", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task<bool> UpdateMstType(int id, ReqUpdateMstTypeDto dto)
        {
            var client = CreateClient();
            var json = JsonSerializer.Serialize(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"master/general/type/{id}", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteMstType(int id, string deletedBy)
        {
            var client = CreateClient();
            var response = await client.DeleteAsync($"master/general/type/{id}?deletedBy={Uri.EscapeDataString(deletedBy)}");
            return response.IsSuccessStatusCode;
        }
    }
}
