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
        public async Task CreateMstModel(ReqCreateMstModelDto dto)
        {
            var client = CreateClient();
            var json = JsonSerializer.Serialize(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await client.PostAsync("master/general/model", content);

            // Jangan gunakan EnsureSuccessStatusCode() agar kita bisa baca isi errornya
            if (!response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                
                try 
                {
                    // Deserialize ke class wrapper API kamu
                    var result = JsonSerializer.Deserialize<ApiClientResponse<object>>(responseContent, _jsonOptions);
                    
                    // Ambil pesan spesifik: "Brand 'MZD', Type 'CRS', and Model 'CX5' is already exists!"
                    throw new Exception(result?.Message ?? "Internal Server Error");
                }
                catch (JsonException)
                {
                    // Jika response bukan JSON (error server mentah)
                    throw new Exception("A system error occurred. Please try again later.");
                }
            }
        }
        public async Task<bool> DeleteMstModel(int id)
        {
            var client = CreateClient();
            var response = await client.DeleteAsync($"master/general/model/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<PagedResult<ResMstModelDto>> GetMstModel(ReqGetModelDto dto)
        {
            var client = CreateClient();
            var url = $"master/general/model?page={dto.Page}&limit={dto.Limit}&search={Uri.EscapeDataString(dto.Search ?? string.Empty)}&IsDoNotHaveStock={dto.IsDoNotHaveStock}";
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

        public async Task<bool> UpdateMstModel(ReqUpdateMstModelDto dto, int id)
        {
            var client = CreateClient();
            var json = JsonSerializer.Serialize(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"master/general/model/{id}", content);
            if (!response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                
                try 
                {
                    // Deserialize ke class wrapper API kamu
                    var result = JsonSerializer.Deserialize<ApiClientResponse<object>>(responseContent, _jsonOptions);
                    
                    throw new Exception(result?.Message ?? "Internal Server Error");
                }
                catch (JsonException)
                {
                    // Jika response bukan JSON (error server mentah)
                    throw new Exception("A system error occurred. Please try again later.");
                }
            }
            return true;
        }
    }
}