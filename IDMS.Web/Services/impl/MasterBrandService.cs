using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using IDMS.Module.Master.Dto.Request;
using IDMS.Module.Master.Dto.Response;
using IDMS.Shared.Entities;
using IDMS.Web.Models;

namespace IDMS.Web.Services.Impl
{
    public class MasterBrandService : BaseClientService, IMstBrandService
    {
        // private readonly IHttpClientFactory _httpClientFactory;
        // private readonly IHttpContextAccessor _httpContextAccessor;

        // private static readonly JsonSerializerOptions _jsonOptions = new()
        // {
        //     PropertyNameCaseInsensitive = true
        // };

        // public MasterBrandService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        // {
        //     _httpClientFactory = httpClientFactory;
        //     _httpContextAccessor = httpContextAccessor;
        // }

        // private HttpClient CreateClient()
        // {
        //     var client = _httpClientFactory.CreateClient("IDMSApi");

        //     // Get the token from the current HTTP context
        //     var token = _httpContextAccessor.HttpContext?.Session.GetString("Token");

        //     if (!string.IsNullOrEmpty(token))
        //     {
        //         client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //     }
        //     return client;
        // }

        public MasterBrandService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        : base(httpClientFactory, httpContextAccessor)
        { }
        public async Task<PageResult<RestMstBrandDto>> GetMstBrand(ReqBaseParamDto dto)
        {
            var client = CreateClient();
            var url = $"master/general/brand?page={dto.Page}&limit={dto.Limit}&search={Uri.EscapeDataString(dto.Search ?? string.Empty)}";
            var response = await client.GetAsync(url);
            var result = await HandleResponse<ApiClientResponse<List<RestMstBrandDto>>>(response);

            return new PageResult<RestMstBrandDto>
            {
                Items = result?.Data ?? new List<RestMstBrandDto>(),
                Pagination = result?.Pagination ?? new Pagination()
            };
        }




        public async Task<RestMstBrandDto?> GetMstBrandById(int id)
        {
            var client = CreateClient();
            var response = await client.GetAsync($"master/general/brand/{id}");
            var result = await HandleResponse<ApiClientResponse<RestMstBrandDto>>(response);
            return result?.Data;
        }

        public async Task CreateMstBrand(ReqCreateMstBrandDto dto)
        {
            var client = CreateClient();
            var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("master/general/brand", content);
            await HandleResponse(response);
        }

        public async Task UpdateNameMstBrand(int id, ReqUpdateNameMstBrandDto dto)
        {
            var client = CreateClient();
            var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"master/general/brand/{id}", content);
            await HandleResponse(response);
        }

        // public async Task<bool> DeleteMstBrand(int id)
        // {
        //     var client = CreateClient();
        //     var response = await client.DeleteAsync($"master/general/brand/{id}");
        //     return response.IsSuccessStatusCode;
        // }
    }
}