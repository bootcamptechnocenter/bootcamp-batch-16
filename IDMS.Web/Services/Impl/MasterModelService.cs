using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using IDMS.Modules.Master.Dto.Request;
using IDMS.Modules.Master.Dto.Response;
using IDMS.Shared.Entities;

namespace IDMS.Web.Services.Impl
{
    public class MasterModelService : IMasterModelService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private static readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

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
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return client;
        }

        public async Task<PagedResult<ResMstModelDto>> GetMstModel(ReqBaseParamDto dto)
        {
            var client = CreateClient();
            var response = await client.GetAsync($"api/MstModel?page={dto.Page}&limit={dto.Limit}&search={Uri.EscapeDataString(dto.Search ?? string.Empty)}");
            if (!response.IsSuccessStatusCode) return new PagedResult<ResMstModelDto>();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<PagedResult<ResMstModelDto>>(content, _jsonOptions) ?? new PagedResult<ResMstModelDto>();
        }

        public async Task<ResMstModelDto?> GetMstModelById(int id)
        {
            var client = CreateClient();
            var response = await client.GetAsync($"api/MstModel/{id}");
            if (!response.IsSuccessStatusCode) return null;
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ResMstModelDto>(content, _jsonOptions);
        }

        // --- 3 FUNGSI INI YANG TADI DICARI SAMA .NET ---

        public async Task CreateMstModel(ReqCreateMstModelDto dto)
        {
            var client = CreateClient();
            var json = JsonSerializer.Serialize(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/MstModel", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task<bool> UpdateMstModel(ReqUpdateMstModelDto dto, int id)
        {
            var client = CreateClient();
            var json = JsonSerializer.Serialize(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"api/MstModel/{id}", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteMstModel(int id)
        {
            var client = CreateClient();
            var response = await client.DeleteAsync($"api/MstModel/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}