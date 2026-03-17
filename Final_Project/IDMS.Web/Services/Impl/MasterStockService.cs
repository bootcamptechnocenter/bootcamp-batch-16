using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using IDMS.Modules.Master.Dto.Request;
using IDMS.Modules.Master.Dto.Response;
using IDMS.Modules.Master.Services;
using IDMS.Shared.Entities;
using IDMS.Web.Models;

namespace IDMS.Web.Services.Impl
{
    public class MasterStockService : IMasterStockService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };
        public MasterStockService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
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

        public async Task<PagedResult<ResMstStockDto>> GetMstStock(ReqBaseParamDto dto)
        {
            var client = CreateClient();
            var url = $"master/general/stock?page={dto.Page}&limit={dto.Limit}&search={Uri.EscapeDataString(dto.Search ?? string.Empty)}";
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiClientResponse<List<ResMstStockDto>>>(content, _jsonOptions);

            return new PagedResult<ResMstStockDto>
            {
                Items = result?.Data ?? new List<ResMstStockDto>(),
                Pagination = result?.Pagination ?? new Pagination()
            };
        }

        public async Task<ResMstStockDto?> GetMstStockById(int id)
        {
            var client = CreateClient();
            var response = await client.GetAsync($"master/general/stock/{id}");
            if (!response.IsSuccessStatusCode) return null;

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiClientResponse<ResMstStockDto>>(content, _jsonOptions);
            return result?.Data;
        }

        public async Task CreateMstStock(ReqCreateMstStockDto dto)
        {
            var client = CreateClient();
            var json = JsonSerializer.Serialize(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await client.PostAsync("master/general/stock", content);

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

        public async Task<bool> UpdateMstStock(ReqUpdateMstStockDto dto, int id)
        {
            var client = CreateClient();
            var json = JsonSerializer.Serialize(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"master/general/stock/{id}", content);
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

        public async Task<bool> DeleteMstStock(int id)
        {
            var client = CreateClient();
            var response = await client.DeleteAsync($"master/general/stock/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}