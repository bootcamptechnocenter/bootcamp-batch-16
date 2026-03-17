using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Modules.Master.Dto.Request;
using IDMS.Modules.Master.Dto.Response;
using IDMS.Shared.Entities;

namespace IDMS.Web.Services
{
    public interface IMasterBrandService
    {
        Task<PagedResult<ResMstBrandDto>> GetMstBrand(ReqBaseParamDto dto);
        Task<ResMstBrandDto?> GetMstBrandById(int id);
        Task CreateMstBrand(ReqCreateMstBrancDto dto);
        Task<bool> UpdateMstBrand(int id, ReqUpdateMstBrancDto dto);
        Task<bool> DeleteMstBrand(int id, string deletedBy);
    }
}