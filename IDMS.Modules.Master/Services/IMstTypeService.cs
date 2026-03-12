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
        Task<PagedResult<ResMstTypeDto>> GetMstTypes(ReqBaseParamDto dto);
        Task<ResMstTypeDto> GetMstTypeById(int id);
        Task CreateMstType(ReqCreateMstTypeDto dto);
        Task UpdateMstType(int id, ReqUpdateMstTypeDto dto);
        Task<bool> DeleteMstType(int id);
    }
}