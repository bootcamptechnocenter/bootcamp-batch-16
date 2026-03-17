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
        Task<bool> UpdateMstModel(int id, ReqUpdateMstModelDto dto);
        Task<bool> DeleteMstModel(int id, string deletedBy);
    }
}
