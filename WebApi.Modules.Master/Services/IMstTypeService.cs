using WebApi.Modules.Master.Dto.Request;
using WebApi.Modules.Master.Dto.Response;
using WebApi.Shared.Entities;

namespace WebApi.Modules.Master.Services
{
    public interface IMstTypeService
    {
        Task<PagedResult<ResMstTypeDto>> GetMstTypes(ReqBaseParamDto dto);
        Task<ResMstTypeDto> GetMstTypeById(int id);
        Task<ResMstTypeDto> CreateMstType(ReqMstTypeDto dto);
        Task<ResMstTypeDto> UpdateMstType(int id, ReqMstTypeUpdateDto dto);
        Task ToggleActiveMstType(int id);
        Task DeleteMstType(int id);
    }
}