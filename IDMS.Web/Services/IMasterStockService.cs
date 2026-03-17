using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        Task<bool> UpdateMstStock(int id, ReqUpdateMstStockDto dto);
        Task<bool> DeleteMstStock(int id);
    }
}