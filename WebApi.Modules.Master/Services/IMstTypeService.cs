using WebApi.Modules.Master.Dto.Response;
using WebApi.Shared.Entities;

namespace WebApi.Modules.Master.Services
{
    public interface IMstTypeService
    {
        Task<PagedResult<ResMstTypeDto>> GetMstTypes(ReqBaseParamDto dto);
    }
}