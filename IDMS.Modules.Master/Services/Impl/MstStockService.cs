using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Infrastructure.Data;
using IDMS.Modules.Master.Dto.Request;
using IDMS.Modules.Master.Dto.Response;
using IDMS.Shared.Domain.Entities;
using IDMS.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace IDMS.Modules.Master.Services.Impl
{
    public class MstStockService: IMstStockService
    {
        private readonly AppDbContext _context;

        public MstStockService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<ResMstStockDto>> GetMstStock(ReqBaseParamDto dto)
        {
            var query = _context.MstStock
                .Include(x => x.Model)
                    .ThenInclude(m => m.Type)
                        .ThenInclude(t => t.Brand)
                .AsQueryable();

            query = query.Where(x => x.DeletedAt == null);

            if (!string.IsNullOrWhiteSpace(dto.Search))
            {
                var search = dto.Search.ToLower();

                query = query.Where(x =>
                    (x.Model != null && x.Model.Name.ToLower().Contains(search)) ||
                    (x.Model != null && x.Model.Type != null && x.Model.Type.Name.ToLower().Contains(search)) ||
                    (x.Model != null && x.Model.Type != null && x.Model.Type.Brand != null && x.Model.Type.Brand.Name.ToLower().Contains(search))
                );
            }

            var totalCount = await query.CountAsync();

            var page = dto.Page > 0 ? dto.Page : 1;
            var pageSize = dto.Limit > 0 ? dto.Limit : 10;
            var skip = (page - 1) * pageSize;
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var items = await query
                .OrderBy(x => x.ModelId)
                .ThenBy(x => x.Id)
                .Skip(skip)
                .Take(pageSize)
                .Select(x => new ResMstStockDto
                {
                    Id = x.Id,

                    ModelId = x.ModelId,
                    ModelCode = x.Model != null ? x.Model.Code : string.Empty,
                    ModelName = x.Model != null ? x.Model.Name : string.Empty,

                    TypeId = x.Model != null ? x.Model.TypeId : 0,
                    TypeCode = x.Model != null && x.Model.Type != null ? x.Model.Type.Code : string.Empty,
                    TypeName = x.Model != null && x.Model.Type != null ? x.Model.Type.Name : string.Empty,

                    BrandId = x.Model != null && x.Model.Type != null ? x.Model.Type.BrandId : 0,
                    BrandCode = x.Model != null && x.Model.Type != null && x.Model.Type.Brand != null 
                        ? x.Model.Type.Brand.Code : string.Empty,
                    BrandName = x.Model != null && x.Model.Type != null && x.Model.Type.Brand != null 
                        ? x.Model.Type.Brand.Name : string.Empty,

                    // STOCK
                    StockAmount = x.StockAmount,
                    Price = x.Price
                })
                .ToListAsync();

            return new PagedResult<ResMstStockDto>
            {
                Items = items,
                Pagination = new Pagination
                {
                    TotalItems = totalCount,
                    CurrentPage = page,
                    Limit = pageSize,
                    TotalPages = totalPages
                }
            };
        }

        public async Task<ResMstStockDto?> GetMstStockById(int id)
        {
            var stock = await _context.MstStock
                .Include(x => x.Model)
                    .ThenInclude(m => m.Type)
                        .ThenInclude(t => t.Brand)
                .Where(x => x.Id == id && x.DeletedAt == null)
                .Select(x => new ResMstStockDto
                {
                    Id = x.Id,

                    ModelId = x.ModelId,
                    ModelCode = x.Model != null ? x.Model.Code : string.Empty,
                    ModelName = x.Model != null ? x.Model.Name : string.Empty,

                    TypeId = x.Model != null ? x.Model.TypeId : 0,
                    TypeCode = x.Model != null && x.Model.Type != null ? x.Model.Type.Code : string.Empty,
                    TypeName = x.Model != null && x.Model.Type != null ? x.Model.Type.Name : string.Empty,

                    BrandId = x.Model != null && x.Model.Type != null ? x.Model.Type.BrandId : 0,
                    BrandCode = x.Model != null && x.Model.Type != null && x.Model.Type.Brand != null
                        ? x.Model.Type.Brand.Code : string.Empty,
                    BrandName = x.Model != null && x.Model.Type != null && x.Model.Type.Brand != null
                        ? x.Model.Type.Brand.Name : string.Empty,

                    StockAmount = x.StockAmount,
                    Price = x.Price
                })
                .FirstOrDefaultAsync();

            return stock;
        }

        public async Task CreateMstStock(ReqCreateMstStockDto dto)
        {
            var stock = new MstStock
            {
                ModelId = dto.ModelId,
                StockAmount = dto.AmountStock,
                Price = dto.Price,
                CreatedAt = DateTime.Now,
                CreatedBy = dto.CreatedBy
            };

            _context.MstStock.Add(stock);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateMstStock(ReqUpdateMstStockDto dto, int id)
        {
            var stock = await _context.MstStock
                .FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);
            if (stock == null) return false;

            stock.ModelId = dto.ModelId;
            stock.StockAmount = dto.StockAmount;
            stock.Price = dto.Price;
            stock.UpdatedAt = DateTime.Now;
            stock.UpdatedBy = dto.UpdatedBy;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteMstStock(int id)
        {
            var stock = await _context.MstStock
                .FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);
            if (stock == null) return false;

            stock.DeletedAt = DateTime.Now;
            stock.DeletedBy = "system";

            await _context.SaveChangesAsync();
            return true;
        }
    }
}