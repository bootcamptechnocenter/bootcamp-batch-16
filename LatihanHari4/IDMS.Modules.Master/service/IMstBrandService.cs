using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDMS.Modules.Master.service
{
    public interface IMstBrandService
    {
        Task<PagedResult<ResMstBrandDto>> GetMstBrand(ReqBaseParamDto dto);
    }
}