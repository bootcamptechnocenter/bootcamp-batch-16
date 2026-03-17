using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Modules.Master.Dto.Request;
using IDMS.Modules.Master.Dto.Response;
using IDMS.Shared.Entities;

namespace IDMS.Modules.Master.Services
{
    public interface IMstStocksService
    {
        public Task<PagedResult<ResMstStocksDto>> GetAllStocks(ReqBaseParamDto dto);

        public Task<ResMstStocksDto?> GetStockById(int id);

        public Task<bool> CreateStock(ReqCreateStockDto dto);

        public Task<bool> UpdateStock(int id, ReqUpdateStockDto dto);
        public Task<bool> DeleteStock(int id);
    }
}