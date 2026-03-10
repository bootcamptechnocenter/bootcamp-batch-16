using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Module.Master.Dto.Response;
using IDMS.Shared.Entities;

namespace IDMS.Module.Master.Services
{
    public interface IMstBrandService
    {
        Task<PageResult<RestMstBrandDto>> GetMstBrand(ReqBaseParamDto param);

    }
}