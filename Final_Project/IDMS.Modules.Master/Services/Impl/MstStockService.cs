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
    public class MstStockService : IMstStockService
    {
        private readonly AppDbContext _context;
        public MstStockService(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateMstStock(ReqCreateMstStockDto dto)
        {
            var targetModel = await _context.MstModels
            .FirstOrDefaultAsync(t => t.Id == dto.ModelId && t.DeletedAt == null);

            if (targetModel == null)
            {
                throw new Exception("Model is not found.");
            }

            var stockData = await _context.MstStocks
            .FirstOrDefaultAsync(t => t.ModelId == dto.ModelId && t.DeletedAt == null);
            
            if (stockData != null)
            {
                throw new Exception($"Stock for model '{targetModel.Name}' is already exists.");
            }

            var stock = new MstStocks
            {
                ModelId = dto.ModelId,
                Price = dto.Price,
                JumlahStock = dto.JumlahStock,
                CreatedAt = DateTime.Now,
                CreatedBy = dto.CreatedBy
            };

            _context.MstStocks.Add(stock);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteMstStock(int id)
        {
            var stock = await _context.MstStocks
            .FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);
            if (stock == null) return false;


            stock.DeletedAt = DateTime.Now;
            stock.DeletedBy = "System";

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<PagedResult<ResMstStockDto>> GetMstStock(ReqBaseParamDto dto)
        {
            
            var query = _context.MstStocks
                .Include(x => x.Model)
                .ThenInclude(x => x.Type)
                .ThenInclude(x => x.Brand)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(dto.Search))
            {
                var search = dto.Search.ToLower();
                query = query.Where(x =>
                    (x.Model != null && x.Model.Code.ToLower().Contains(search)) ||
                    (x.Model != null && x.Model.Name.ToLower().Contains(search)) ||
                    (x.Model != null && x.Model.Type != null && x.Model.Type.Name.ToLower().Contains(search)) ||
                    (x.Model != null && x.Model.Type != null && x.Model.Type.Name.ToLower().Contains(search)) ||
                    (x.Model != null && x.Model.Type != null && x.Model.Type.Brand != null && x.Model.Type.Brand.Name.ToLower().Contains(search)) ||
                    (x.Model != null && x.Model.Type != null && x.Model.Type.Brand != null && x.Model.Type.Brand.Code.ToLower().Contains(search))
                );
            }

            query = query.Where(x => x.DeletedAt == null);

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
                    TypeCode = x.Model.Type != null ? x.Model.Type.Code : string.Empty,
                    TypeName = x.Model.Type != null ? x.Model.Type.Name : string.Empty,
                    BrandCode = x.Model.Type.Brand  != null ? x.Model.Type.Brand.Code : string.Empty,
                    BrandName = x.Model.Type.Brand != null ? x.Model.Type.Brand.Name : string.Empty,
                    JumlahStock = x.JumlahStock,
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
            var stock = await _context.MstStocks
                .Include(x => x.Model)
                .ThenInclude(x => x.Type)
                .ThenInclude(x => x.Brand)
                .Where(x => x.Id == id && x.DeletedAt == null)
                .Select(x => new ResMstStockDto
                {
                    Id = x.Id,
                    ModelId = x.ModelId,
                    ModelCode = x.Model != null ? x.Model.Code : string.Empty,
                    ModelName = x.Model != null ? x.Model.Name : string.Empty, 
                    TypeCode = x.Model.Type != null ? x.Model.Type.Code : string.Empty,
                    TypeName = x.Model.Type != null ? x.Model.Type.Name : string.Empty,
                    BrandCode = x.Model.Type.Brand  != null ? x.Model.Type.Brand.Code : string.Empty,
                    BrandName = x.Model.Type.Brand != null ? x.Model.Type.Brand.Name : string.Empty,
                    JumlahStock = x.JumlahStock,
                    Price = x.Price
                })
                .FirstOrDefaultAsync();

            return stock;
        }

        public async Task<bool> UpdateMstStock(ReqUpdateMstStockDto dto, int id)
        {
            var stock = await _context.MstStocks
                .FirstOrDefaultAsync(t => t.Id == id && t.DeletedAt == null);

            if (stock == null)
            {
                throw new Exception("Stock is not found.");
            }

            stock.Price = dto.Price;
            stock.JumlahStock = dto.JumlahStock;
            stock.UpdatedAt = DateTime.Now;
            stock.UpdatedBy = dto.UpdatedBy;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}