using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Modules.Master.Dto.Request;
using IDMS.Modules.Master.Dto.Response;
using IDMS.Shared.Entities;

namespace IDMS.Web.Services
{
    public interface IMasterModelService
    {
        Task<PagedResult<ResMstModelDto>> GetMstModel(ReqBaseParamDto dto);
        Task<ResMstModelDto?> GetMstModelById(int id);
        Task CreateMstModel(ReqCreateMstModelDto dto);

        Task<bool> UpdateMstModel(ReqUpdateMstModelDto dto, int id);
        Task<bool> DeleteMstModel(int id);
        
    }
}