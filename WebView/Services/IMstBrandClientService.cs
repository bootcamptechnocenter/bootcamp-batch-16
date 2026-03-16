using WebApi.Modules.Master.Dto.Request;
using WebApi.Modules.Master.Dto.Response;
using WebApi.Shared.Entities;

namespace WebView.Services
{
    public interface IMstBrandClientService
    {
        Task<PagedResult<ResMstBrandDto>> GetMstBrands(ReqBaseParamDto dto);
        Task<ResMstBrandDto> GetMstBrandById(int id);
        Task<ResMstBrandDto> CreateMstBrand(ReqMstBrandDto dto);
        Task<ResMstBrandDto> UpdateMstBrand(int id, ReqMstBrandUpdateDto dto);
        Task DeleteMstBrand(int id);
    }
}