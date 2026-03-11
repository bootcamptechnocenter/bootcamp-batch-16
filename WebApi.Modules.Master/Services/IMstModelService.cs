using WebApi.Modules.Master.Dto.Request;
using WebApi.Modules.Master.Dto.Response;
using WebApi.Shared.Entities;

namespace WebApi.Modules.Master.Services
{
    public interface IMstModelService
    {
        Task<PagedResult<ResMstModelDto>> GetMstModels(ReqBaseParamDto dto);
        Task<ResMstModelDto> GetMstModelById(int id);
        Task<ResMstModelDto> CreateMstModel(ReqMstModelDto dto);
        Task<ResMstModelDto> UpdateMstModel(int id, ReqMstModelUpdateDto dto);
        Task DeleteMstModel(int id);
    }
}