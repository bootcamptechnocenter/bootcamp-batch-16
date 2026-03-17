using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Modules.Master.Dto.Request;
using IDMS.Modules.Master.Dto.Response;
using IDMS.Shared.Entities;

namespace IDMS.Modules.Master.Services
{
    public interface IMstTypeService
    {
        Task<PagedResult<ResMstTypeDto>> GetMstType(ReqBaseParamDto dto);
        Task<ResMstTypeDto?> GetMstTypeById(int id);
        Task CreateMstType(ReqCreateMstTypeDto dto);
        Task<bool> UpdateMstType(ReqUpdateMstTypeDto dto, int id);
        Task<bool> DeleteMstType(int id);
    }
}
