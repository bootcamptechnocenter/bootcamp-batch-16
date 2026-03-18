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

namespace IDMS.Web.Services.impl
{
    public class MasterStockService : BaseClientService, IMstStockService
    {
        public MasterStockService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
       : base(httpClientFactory, httpContextAccessor)
        { }


        public async Task<PageResult<RestMstStockDto>> GetMstStock(ReqBaseParamDto dto)
        {
            var client = CreateClient();
            var url = $"master/stock?page={dto.Page}&limit={dto.Limit}&search={Uri.EscapeDataString(dto.Search ?? string.Empty)}";
            var response = await client.GetAsync(url);
            var result = await HandleResponse<ApiClientResponse<List<RestMstStockDto>>>(response);

            return new PageResult<RestMstStockDto>
            {
                Items = result?.Data ?? new List<RestMstStockDto>(),
                Pagination = result?.Pagination ?? new Pagination()
            };
        }

        public async Task UpsertMstStock(ReqUpsertMstStockDto dto)
        {
            var client = CreateClient();
            var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("master/stock", content);
            await HandleResponse<ApiClientResponse<object>>(response);
        }

        public async Task<bool> DeleteMstStock(int id)
        {
            var client = CreateClient();
            var response = await client.DeleteAsync($"master/stock/{id}");
            await HandleResponse<ApiClientResponse<object>>(response);
            return response.IsSuccessStatusCode;
        }

    }
}