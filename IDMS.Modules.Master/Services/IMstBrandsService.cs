using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Modules.Master.Dto.Request;
using IDMS.Modules.Master.Dto.Response;
using IDMS.Shared.Entities;

namespace IDMS.Modules.Master.Services
{
    public interface IMstBrandsService
    {
        Task<PagedResult<ResMstBrandsDto>> GetMstBrands(ReqBaseParamDto dto);
        Task<ResMstBrandsDto?> GetMstBrandsById(int id);
        Task CreateMstBrand(ReqCreateMstBrandDto dto);
        Task<bool> UpdateMstBrand(int id, ReqUpdateMstBrandDto dto);
        Task<bool> DeleteMstBrand(int id);
    }
}