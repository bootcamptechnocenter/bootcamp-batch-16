using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Modules.Master.Dto.Request;
using IDMS.Modules.Master.Dto.Response;
using IDMS.Shared.Entities;

namespace IDMS.Modules.Master.Services
{
    public interface IMstTypesService
    {
        public Task<PagedResult<ResMstTypeDto>> GetAllTypes(ReqBaseParamDto dto);

        public Task<ResMstTypeDto?> GetTypeById(int id);

        public Task<bool> CreateType(ReqCreateTypeDto dto);

        public Task<bool> UpdateType(int id, ReqUpdateTypeDto dto);
        public Task<bool> DeleteType(int id);
    }
}