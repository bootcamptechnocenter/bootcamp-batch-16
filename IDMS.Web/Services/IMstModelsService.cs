using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Modules.Master.Dto.Request;
using IDMS.Modules.Master.Dto.Response;
using IDMS.Shared.Entities;

namespace IDMS.Web.Services
{
    public interface IMstModelsService
    {
        Task<PagedResult<ResMstModelsDto>> GetAllModels(ReqBaseParamDto dto);
        Task<ResMstModelsDto?> GetModelById(int id);
        Task<bool> CreateModel(ReqCreateModelDto dto);
        Task<bool> UpdateModel(int id, ReqUpdateModelDto dto);
        Task<bool> DeleteModel(int id);
    }
}