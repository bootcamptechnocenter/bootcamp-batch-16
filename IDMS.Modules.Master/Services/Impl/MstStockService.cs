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
        public async Task<PagedResult<ResMstStockDto>> GetMstStock(ReqBaseParamDto dto)
        {
            // var query = _context.MstStocks.AsQueryable();
            var query = _context.MstStocks
                .AsNoTracking()
                .Include(x => x.Model)
                    .ThenInclude(m => m.Type)
                        .ThenInclude(t => t.Brand)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(dto.Search))
            {
                var search = dto.Search.ToLower();
                query = query.Where(x =>
                    (x.Models.Code != null && x.Models.Code.ToLower().Contains(search)) ||
                    (x.Models.Name != null && x.Models.Name.ToLower().Contains(search)) ||
                    (x.Models.Type.Brand.Code != null && x.Models.Type.Brand.Code.ToLower().Contains(search)) ||
                    (x.Models.Type.Brand.Name != null && x.Models.Type.Brand.Name.ToLower().Contains(search)) 
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
                .Select(x => new ResMstStockDto
                {
                    Id = x.Id,

                    ModelId = x.ModelId,
                    ModelCode = x.Model.Code,
                    ModelName = x.Model.Name,

                    TypeId = x.Model.TypeId,
                    TypeCode = x.Model.Type.Code,
                    TypeName = x.Model.Type.Name,

                    BrandId = x.Model.Type.BrandId,
                    BrandCode = x.Model.Type.Brand.Code,
                    BrandName = x.Model.Type.Brand.Name,

                    JumlahStock = x.JumlahStock,
                    Harga = x.Harga
                })
                .ToListAsync();

            return new PagedResult<ResMstStockDto>
            {
                Items = items,
                Pagination = new Pagination()
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
            var model = await _context.MstStocks
            .AsNoTracking()
                .Include(x => x.Models)
                .Where(x => x.Id == id && x.DeletedAt == null)
                .Select(x => new ResMstStockDto
                {
                    Id = x.Id,

                    ModelId = x.Model_Id,
                    ModelCode = x.Models != null ? x.Models.Code : string.Empty,
                    ModelName = x.Models != null ? x.Models.Name : string.Empty,

                    TypeId = x.Models != null ? x.Models.TypeId : 0,
                    TypeCode = x.Models != null && x.Models.Type != null ? x.Models.Type.Code : string.Empty,
                    TypeName = x.Models != null && x.Models.Type != null ? x.Models.Type.Name : string.Empty,

                    BrandId = x.Models != null && x.Models.Type != null ? x.Models.Type.BrandId : 0,
                    BrandCode = x.Models != null && x.Models.Type != null && x.Models.Type.Brand != null ? x.Models.Type.Brand.Code : string.Empty,
                    BrandName = x.Models != null && x.Models.Type != null && x.Models.Type.Brand != null ? x.Models.Type.Brand.Name : string.Empty,

                    JumlahStock = x.JumlahStock,
                    Harga = x.Harga
                })
                .FirstOrDefaultAsync();

            return model;
        }

        public async Task CreateMstStock(ReqCreateMstStockDto dto)
        {
            var model = new MstStocks
            {
                ModelId = dto.ModelId,
                JumlahStock = dto.JumlahStock,
                Harga = dto.Harga,
                CreatedAt = DateTime.Now,
                CreatedBy = dto.CreatedBy
            };

            _context.MstStocks.Add(model);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateMstStock(int id, ReqUpdateMstStockDto dto)
        {
            var stock = await _context.MstStocks
                .FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);
            if (stock == null) return false;

            stock.ModelId = dto.ModelId;
            stock.JumlahStock = dto.JumlahStock;
            stock.Harga = dto.Harga;
            // model.IsActive = dto.IsActive;
            stock.UpdatedAt = DateTime.Now;
            stock.UpdatedBy = dto.UpdatedBy;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteMstStock(int id)
        {
            var stock = await _context.MstStocks
                .FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);
            if (stock == null) return false;

            stock.DeletedAt = DateTime.Now;
            stock.DeletedBy = "system";

            await _context.SaveChangesAsync();
            return true;
        }
    }
}