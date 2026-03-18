using System.Threading.Tasks;
using IDMS.Modules.Master.Dto.Request;
using IDMS.Modules.Master.Dto.Response;
using IDMS.Shared.Entities;
using IDMS.Modules.Master.Dto.Request;

namespace IDMS.Modules.Master.Services
{
    public interface IMstStockService
    {
        Task<PagedResult<ResMstStockDto>> GetMstStock(ReqBaseParamDto dto);
        Task<ResMstStockDto?> GetMstStockById(int id);
        Task CreateMstStock(ReqCreateMstStockDto dto);
        Task<bool> UpdateMstStock(ReqUpdateMstStockDto dto, int id);
        Task<bool> DeleteMstStock(int id);
    }
}