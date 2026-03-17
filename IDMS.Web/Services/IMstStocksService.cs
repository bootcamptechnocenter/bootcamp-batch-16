using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Modules.Master.Dto.Request;
using IDMS.Modules.Master.Dto.Response;
using IDMS.Shared.Entities;

namespace IDMS.Web.Services
{
    public interface IMstStocksService
    {
        Task<PagedResult<ResMstStocksDto>> GetAllStocks(ReqBaseParamDto dto);
        Task<ResMstStocksDto?> GetStockById(int id);
        Task<bool> CreateStock(ReqCreateStockDto dto);
        Task<bool> UpdateStock(int id, ReqUpdateStockDto dto);
        Task<bool> DeleteStock(int id);
    }
}
