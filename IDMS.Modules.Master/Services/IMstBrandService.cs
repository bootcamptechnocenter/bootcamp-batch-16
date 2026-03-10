using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Modules.Master.Dto.Response;
using IDMS.Shared.Entities;

namespace IDMS.Modules.Master.Services
{
    public interface IMstBrandService
    {
        Task<PagedResult<ResMstBrandDto>> GetMstBrand(ReqBaseParamDto dto);
    }
}