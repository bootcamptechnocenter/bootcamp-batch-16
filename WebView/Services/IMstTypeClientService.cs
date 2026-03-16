using WebApi.Modules.Master.Dto.Request;
using WebApi.Modules.Master.Dto.Response;
using WebApi.Shared.Entities;

namespace WebView.Services
{
    public interface IMstTypeClientService
    {
        Task<PagedResult<ResMstTypeDto>> GetMstTypes(ReqBaseParamDto dto);
        Task<ResMstTypeDto> GetMstTypeById(int id);
        Task<ResMstTypeDto> CreateMstType(ReqMstTypeDto dto);
        Task<ResMstTypeDto> UpdateMstType(int id, ReqMstTypeUpdateDto dto);
        Task DeleteMstType(int id);
    }
}