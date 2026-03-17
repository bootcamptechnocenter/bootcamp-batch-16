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
            var stock = new MstStock
            {
                ModelId = dto.ModelId,
                JumlahStock = dto.JumlahStock,
                Harga = dto.Harga
            };

            _context.MstStocks.Add(stock);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteMstStock(int id)
        {
            var stock = await _context.MstStocks.FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);
            if (stock == null)
            {
                return false;
            }

            _context.MstStocks.Remove(stock);
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
                    (x.Model.Name != null && x.Model.Name.ToLower().Contains(search)) ||
                    (x.Model.Type.Name != null && x.Model.Type.Name.ToLower().Contains(search)) ||
                    (x.Model.Type.Brand.Name != null && x.Model.Type.Brand.Name.ToLower().Contains(search))
                );

            }

            query = query.Where(x => x.DeletedAt == null);

            var totalCount = await query.CountAsync();

            var page = dto.Page > 0 ? dto.Page : 1;
            var pageSize = dto.Limit > 0 ? dto.Limit : 10;
            var skip = (page - 1) * pageSize;
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var items = await query
                .OrderBy(x => x.Id)
                .Skip(skip)
                .Take(pageSize)
                .Select(x => new ResMstStockDto
                {
                    Id = x.Id,
                    ModelId = x.ModelId,
                    ModelName = x.Model.Name,
                    JumlahStock = x.JumlahStock,
                    Harga = x.Harga,
                    TypeId = x.Model.TypeId,
                    TypeName = x.Model.Type.Name,
                    BrandId = x.Model.Type.BrandId,
                    BrandName = x.Model.Type.Brand.Name
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
                    ModelName = x.Model.Name,
                    JumlahStock = x.JumlahStock,
                    Harga = x.Harga,
                    TypeId = x.Model.TypeId,
                    TypeName = x.Model.Type.Name,
                    BrandId = x.Model.Type.BrandId,
                    BrandName = x.Model.Type.Brand.Name
                })
                .FirstOrDefaultAsync();

            return stock;

        }

        public async Task<bool> UpdateMstStock(ReqUpdateMstStockDto dto, int id)
        {
            var stock = await _context.MstStocks.FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);
            if (stock == null)
            {
                return false;
            }

            stock.ModelId = dto.ModelId;
            stock.JumlahStock = dto.JumlahStock;
            stock.Harga = dto.Harga;

            _context.MstStocks.Update(stock);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}