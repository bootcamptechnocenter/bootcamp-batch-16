using WebApi.Modules.Master.Dto.Request;
using WebApi.Modules.Master.Dto.Response;
using WebApi.Shared.Entities;

namespace WebApi.Modules.Master.Services
{
    public interface IMstBrandService
    {
        Task<PagedResult<ResMstBrandDto>> GetMstBrands(ReqBaseParamDto dto);
        Task<ResMstBrandDto> GetMstBrandById(int id);
        Task<ResMstBrandDto> CreateMstBrand(ReqMstBrandDto dto);
        Task<ResMstBrandDto> UpdateMstBrand(int id, ReqMstBrandUpdateDto dto);
    }
}