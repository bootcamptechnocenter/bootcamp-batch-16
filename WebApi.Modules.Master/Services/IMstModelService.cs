using WebApi.Modules.Master.Dto.Response;
using WebApi.Shared.Entities;

namespace WebApi.Modules.Master.Services
{
    public interface IMstModelService
    {
        Task<PagedResult<ResMstModelDto>> GetMstModels(ReqBaseParamDto dto);
    }
}