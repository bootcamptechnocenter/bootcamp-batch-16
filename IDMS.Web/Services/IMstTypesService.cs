using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Modules.Master.Dto.Request;
using IDMS.Modules.Master.Dto.Response;
using IDMS.Shared.Entities;

namespace IDMS.Web.Services
{
    public interface IMstTypesService
    {
        Task<PagedResult<ResMstTypeDto>> GetAllTypes(ReqBaseParamDto dto);
        Task<ResMstTypeDto?> GetTypeById(int id);
        Task<bool> CreateType(ReqCreateTypeDto dto);
        Task<bool> UpdateType(int id, ReqUpdateTypeDto dto);
        Task<bool> DeleteType(int id);
    }
}
