using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Module.Master.Dto.Response;
using IDMS.Shared.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IDMS.Web.Services
{
    public interface IMstModelService
    {
        Task<PageResult<RestMstModelDto>> GetAvailableMstModel(ReqBaseParamDto param);
    }
}