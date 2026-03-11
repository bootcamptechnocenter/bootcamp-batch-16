using WebApi.Modules.Master.Dto.Response;
using WebApi.Shared.Entities;

namespace WebApi.Modules.Master.Services
{
    public interface IMstBrandService
    {
        Task<PagedResult<ResMstBrandDto>> GetMstBrands(ReqBaseParamDto dto);
    }
}