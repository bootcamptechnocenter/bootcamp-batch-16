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
        Task<bool> UpdateMstType(int id, ReqUpdateMstTypeDto dto);
        Task<bool> DeleteMstType(int id, string deletedBy);
    }
}
