using IDMS.Infrastructure.Data;
using IDMS.Modules.Master.Dto.Response;
using Microsoft.EntityFrameworkCore;

namespace IDMS.Modules.Master.Services.Impl
{
    public class DashboardService : IDashboardService
    {
        private readonly AppDbContext _context;

        public DashboardService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResDashboardDto> GetDashboardData()
        {
            // stock ready per brand (IsReady = true, DeletedAt = null)
            var stockPerBrand = await _context.MstStocks
                .Where(s => s.IsReady && s.DeletedAt == null)
                .Join(_context.MstModels,
                    s => s.ModelId,
                    m => m.Id,
                    (s, m) => new { m.TypeId })
                .Join(_context.MstTypes,
                    sm => sm.TypeId,
                    t => t.Id,
                    (sm, t) => new { t.BrandId })
                .Join(_context.MstBrands,
                    st => st.BrandId,
                    b => b.Id,
                    (st, b) => new { b.Name })
                .GroupBy(x => x.Name)
                .Select(g => new ResBrandStockDto
                {
                    BrandName  = g.Key,
                    ReadyCount = g.Count()
                })
                .OrderByDescending(x => x.ReadyCount)
                .ToListAsync();

            var totalRevenue = await _context.MstStocks.Where(x => !x.IsReady && x.DeletedAt == null).SumAsync(x => x.Price);


            return new ResDashboardDto
            {
                StockPerBrand    = stockPerBrand,
                TotalReadyStock  = stockPerBrand.Sum(x => x.ReadyCount),
                TotalRevenue     = totalRevenue
            };
        }
    }
}