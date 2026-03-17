using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Module.Master.Dto.Response;
using IDMS.Shared.Entities;

namespace IDMS.Module.Master.Services
{
    public interface IMstModelService
    {
        Task<PageResult<RestMstModelDto>> GetMstModel(ReqBaseParamDto param);
        Task<PageResult<RestMstModelDto>> GetAvailableMstModel(ReqBaseParamDto param);
    }
}