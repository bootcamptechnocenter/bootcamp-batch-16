using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Module.Master.Dto.Request;
using IDMS.Module.Master.Dto.Response;
using IDMS.Shared.Entities;

namespace IDMS.Web.Services
{
    public interface IMstBrandService
    {
        Task<PageResult<RestMstBrandDto>> GetMstBrand(ReqBaseParamDto param);
        Task<RestMstBrandDto?> GetMstBrandById(int id);
        Task CreateMstBrand(ReqCreateMstBrandDto dto);

        Task UpdateNameMstBrand(int id, ReqUpdateNameMstBrandDto dto);
    }


}