using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Infrastructure.Data;
using IDMS.Module.Master.Dto.Request;
using IDMS.Module.Master.Dto.Response;
using IDMS.Shared.Domain.Entities;
using IDMS.Shared.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace IDMS.Module.Master.Services.Impl
{
    public class MstStockService : BaseService, IMstStockService
    {
        private AppDbContext _context;

        public MstStockService(AppDbContext context, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _context = context;
        }

        public async Task<PageResult<RestMstStockDto>> GetMstStock(ReqBaseParamDto dto)
        {
            var query = _context.MstStocks.Include(x => x.Model!).ThenInclude(t => t.Type).ThenInclude(i => i.Brand).AsQueryable();

            if (!string.IsNullOrEmpty(dto.Search))
            {
                var search = dto.Search.ToLower();

                query = query.Where(x =>
                    (x.Model != null && x.Model.Name != null && x.Model.Name.ToLower().Contains(search)) ||
                    (x.Model != null && x.Model.Code != null && x.Model.Code.ToLower().Contains(search)) ||
                    (x.Model != null && x.Model.Type != null && x.Model.Type.Name != null && x.Model.Type.Name.ToLower().Contains(search)) ||
                    (x.Model != null && x.Model.Type != null && x.Model.Type.Brand != null && x.Model.Type.Brand.Name != null && x.Model.Type.Brand.Name.ToLower().Contains(search))
                );
            }

            query = query.Where(x => x.DeletedAt == null);

            var totalCount = await query.CountAsync();

            var page = dto.Page > 0 ? dto.Page : 1;
            var pageSize = dto.Limit > 0 ? dto.Limit : 10;
            var skip = (page - 1) * pageSize;

            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);


            var items = await query
               .OrderBy(x => x.Id)
               .Skip(skip)
               .Take(pageSize)
               .Select(x => new RestMstStockDto
               {
                   Id = x.Id,
                   TotalStock = x.TotalStock,
                   Price = x.Price,
                   Model = x.Model,
                   MstModelId = x.MstModelId
               })
               .ToListAsync();
            Console.WriteLine($"items: {items}");

            return new PageResult<RestMstStockDto>
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

        public async Task UpsertMstStock(ReqUpsertMstStockDto dto)
        {
            var currentUser = GetCurrentUser();
            var existingStock = await _context.MstStocks.FirstOrDefaultAsync(s => s.MstModelId == dto.ModelId);

            if (existingStock != null)
            {
                // Update existing stock
                existingStock.TotalStock = dto.isAccumulate ? dto.TotalStock + existingStock.TotalStock : dto.TotalStock; // Add to existing stock if isAccumulate is true
                existingStock.Price = dto.Price;
                existingStock.UpdatedAt = DateTime.Now;
                existingStock.UpdatedBy = currentUser;
                existingStock.DeletedAt = null; // In case it was previously deleted
                existingStock.DeletedBy = null;
            }
            else
            {
                // Create new stock
                var newStock = new MstStocks
                {
                    MstModelId = dto.ModelId,
                    TotalStock = dto.TotalStock,
                    Price = dto.Price,
                    CreatedAt = DateTime.Now,
                    CreatedBy = currentUser
                };
                await _context.MstStocks.AddAsync(newStock);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteMstStock(int id)
        {
            var currentUser = GetCurrentUser();
            var stock = await _context.MstStocks
            .FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);
            if (stock == null) return false;


            stock.DeletedAt = DateTime.Now;
            stock.DeletedBy = currentUser;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}