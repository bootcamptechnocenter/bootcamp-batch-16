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
        // get all types 
        Task<PagedResult<ResMstTypeDto>> GetMstTypes(ReqBaseParamDto dto);
        // get type by id
        Task<ResMstTypeDto?> GetMstTypeById(int id);
        // create type
        Task CreateMstType(ReqCreateMstType dto);
        // update type
        Task UpdateMstType(int id, ReqUpdateMstTypeDto dto);
        // delete type
        Task DeleteMstType(int id, string deletedBy);
    }
}