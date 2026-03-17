using IDMS.Modules.Master.Dto.Request;
using IDMS.Modules.Master.Dto.Response;
using IDMS.Shared.Domain.Entities;
using IDMS.Shared.Entities;

namespace IDMS.Modules.Master.Services
{
    public interface IMstStockService
    {
        Task<PagedResult<ResMstStockDto>> GetMstStock(ReqGetMstStockDto dto);
        Task<ResMstStockDto?> GetMstStockById(int id);
        Task CreateMstStock(ReqCreateMstStockDto dto);
        Task<bool> UpdateMstStock(ReqUpdateMstStockDto dto, int id);
        Task<bool> DeleteMstStock(int id);
    }
}