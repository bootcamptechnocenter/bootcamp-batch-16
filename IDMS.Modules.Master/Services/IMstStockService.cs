using IDMS.Modules.Master.Dto.Request;
using IDMS.Modules.Master.Dto.Response;
using IDMS.Shared.Entities;

namespace IDMS.Modules.Master.Services
{
    public interface IMstStockService
    {
        Task<PagedResult<ResMstStockDto>> GetMstStock(ReqBaseParamDto dto);
        Task<ResMstStockDto?> GetMstStockById(int id);
        Task CreateMstStock(ReqCreateMstStockDto dto);
        Task<bool> UpdateMstStock(int id, ReqUpdateMstStockDto dto);
        Task<bool> DeleteMstStock(int id, string deletedBy);
    }
}
