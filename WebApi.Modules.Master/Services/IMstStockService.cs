using WebApi.Modules.Master.Dto.Request;
using WebApi.Modules.Master.Dto.Response;
using WebApi.Shared.Entities;

namespace WebApi.Modules.Master.Services
{
    public interface IMstStockService
    {
        Task<PagedResult<ResMstStockDto>> GetMstStocks(ReqBaseParamDto dto);
        Task<ResMstStockDto> GetMstStockById(int id);
        Task<ResMstStockDto> CreateMstStock(ReqMstStockDto dto);
        Task<ResMstStockDto> UpdateMstStock(int id, ReqMstStockUpdateDto dto);
        Task DeleteMstStock(int id);
    }
}
