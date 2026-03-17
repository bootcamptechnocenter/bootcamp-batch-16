using IDMS.Infrastructure.Data;
using IDMS.Modules.Master.Dto.Request;
using IDMS.Modules.Master.Dto.Response;
using IDMS.Shared.Domain.Entities;
using IDMS.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace IDMS.Modules.Master.Services.Impl
{
    public class MstStockService : IMstStockService
    {
        private readonly AppDbContext _context;

        public MstStockService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<ResMstStockDto>> GetMstStock(ReqBaseParamDto dto)
        {
            var query = _context.MstStocks.AsQueryable();
            if (!string.IsNullOrWhiteSpace(dto.Search))
            {
                var search = dto.Search.ToLower();
                query = query.Where(x => x.Model != null && x.Model.Name.ToLower().Contains(search));
            }

            query = query.Where(x => x.DeletedAt == null);

            var totalCount = await query.CountAsync();

            var page = dto.Page > 0 ? dto.Page : 1;
            var pageSize = dto.Limit > 0 ? dto.Limit : 10;
            var skip = (page - 1) * pageSize;
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var items = await query.OrderBy(x => x.Id)
                .Skip(skip)
                .Take(pageSize)
                .Select(x => new ResMstStockDto
                {
                    Id = x.Id,
                    ModelId = x.ModelId,
                    ModelName = x.Model != null ? x.Model.Name : string.Empty,
                    JumlahStock = x.JumlahStock,
                    Harga = x.Harga
                })
                .ToListAsync();

            return new PagedResult<ResMstStockDto>
            {
                Items = items,
                Pagination = new Pagination
                {
                    CurrentPage = page,
                    Limit = pageSize,
                    TotalItems = totalCount,
                    TotalPages = totalPages
                }
            };
        }

        public async Task<ResMstStockDto?> GetMstStockById(int id)
        {
            var stock = await _context.MstStocks
                .Where(x => x.Id == id && x.DeletedAt == null)
                .Select(x => new ResMstStockDto
                {
                    Id = x.Id,
                    ModelId = x.ModelId,
                    ModelName = x.Model != null ? x.Model.Name : string.Empty,
                    JumlahStock = x.JumlahStock,
                    Harga = x.Harga
                })
                .FirstOrDefaultAsync();

            return stock;
        }

        public async Task CreateMstStock(ReqCreateMstStockDto dto)
        {
            var stock = new MstStock
            {
                ModelId = dto.ModelId,
                JumlahStock = dto.JumlahStock,
                Harga = dto.Harga,
                CreatedAt = DateTime.Now,
                CreatedBy = "Admin"
            };

            _context.MstStocks.Add(stock);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateMstStock(int id, ReqUpdateMstStockDto dto)
        {
            var stock = await _context.MstStocks.FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);
            if (stock == null) return false;

            stock.ModelId = dto.ModelId;
            stock.JumlahStock = dto.JumlahStock;
            stock.Harga = dto.Harga;
            stock.UpdatedAt = DateTime.Now;
            stock.UpdatedBy = string.IsNullOrWhiteSpace(dto.UpdatedBy) ? "Admin" : dto.UpdatedBy;

            _context.MstStocks.Update(stock);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteMstStock(int id, string deletedBy)
        {
            var stock = await _context.MstStocks.FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);
            if (stock == null) return false;

            stock.DeletedAt = DateTime.Now;
            stock.DeletedBy = string.IsNullOrWhiteSpace(deletedBy) ? "Admin" : deletedBy;

            _context.MstStocks.Update(stock);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
