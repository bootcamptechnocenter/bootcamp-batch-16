using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Module.Master.Dto.Response;
using IDMS.Shared.Entities;
using IDMS.Web.Models;

namespace IDMS.Web.Services.impl
{
    public class MasterModelService : BaseClientService, IMstModelService
    {
        public MasterModelService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
       : base(httpClientFactory, httpContextAccessor)
        { }

        public async Task<PageResult<RestMstModelDto>> GetAvailableMstModel(ReqBaseParamDto dto)
        {
            var client = CreateClient();
            var url = $"master/model?page={dto.Page}&limit={dto.Limit}&search={Uri.EscapeDataString(dto.Search ?? string.Empty)}";
            var response = await client.GetAsync(url);
            var result = await HandleResponse<ApiClientResponse<List<RestMstModelDto>>>(response);

            return new PageResult<RestMstModelDto>
            {
                Items = result?.Data ?? new List<RestMstModelDto>(),
                Pagination = result?.Pagination ?? new Pagination()
            };
        }
    }
}