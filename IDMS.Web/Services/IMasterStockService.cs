using IDMS.Modules.Master.Dto.Request;
using IDMS.Modules.Master.Dto.Response;
using IDMS.Shared.Entities;

namespace IDMS.Web.Services
{
    public interface IMasterStockService
    {
        Task<PagedResult<ResMstStockDto>> GetMstStock(ReqBaseParamDto dto);
        Task<ResMstStockDto?> GetMstStockById(int id);
        Task CreateMstStock(ReqCreateMstStockDto dto);
        Task<bool> UpdateMstStock(ReqUpdateMstStockDto dto, int id);
        Task<bool> DeleteMstStock(int id);
    }
}