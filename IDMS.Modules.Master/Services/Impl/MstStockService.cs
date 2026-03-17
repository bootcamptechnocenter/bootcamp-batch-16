using System;
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

        public async Task<PagedResult<ResMstStockDto>> GetMstStock(ReqGetMstStockDto dto)
        {
            var query = _context.MstStocks
                .Include(x => x.Model)
                    .ThenInclude(m => m.Type)
                        .ThenInclude(t => t.Brand)
                .AsQueryable();

            // filter by ModelId jika ada
            if (dto.ModelId.HasValue && dto.ModelId.Value > 0)
                query = query.Where(x => x.ModelId == dto.ModelId.Value);

            if (!string.IsNullOrWhiteSpace(dto.Search))
            {
                var search = dto.Search.ToLower();
                query = query.Where(x =>
                    (x.Color != null && x.Color.ToLower().Contains(search)) ||
                    (x.PoliceNumber != null && x.PoliceNumber.ToLower().Contains(search)) ||
                    (x.Model != null && x.Model.Code.ToLower().Contains(search)) ||
                    (x.Model != null && x.Model.Name.ToLower().Contains(search)) ||
                    (x.Model != null && x.Model.Type != null && x.Model.Type.Code.ToLower().Contains(search)) ||
                    (x.Model != null && x.Model.Type != null && x.Model.Type.Name.ToLower().Contains(search)) ||
                    (x.Model != null && x.Model.Type != null && x.Model.Type.Brand != null && x.Model.Type.Brand.Name.ToLower().Contains(search))
                );
            }

            query = query.Where(x => x.DeletedAt == null);

            var totalCount = await query.CountAsync();

            var page      = dto.Page > 0 ? dto.Page : 1;
            var pageSize  = dto.Limit > 0 ? dto.Limit : 10;
            var skip      = (page - 1) * pageSize;
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            // sorting priority: IsReady
            var items = await query
                .OrderBy(x => x.IsReady? 0 : 1)
                .ThenBy(x => x.ModelId)
                .ThenBy(x => x.Id)
                .Skip(skip)
                .Take(pageSize)
                .Select(x => new ResMstStockDto
                {
                    Id           = x.Id,
                    Color        = x.Color,
                    PoliceNumber = x.PoliceNumber,
                    NewUsed      = x.NewUsed,
                    Price        = x.Price,
                    IsReady      = x.IsReady,
                    ModelId      = x.ModelId,
                    ModelCode    = x.Model != null ? x.Model.Code : string.Empty,
                    ModelName    = x.Model != null ? x.Model.Name : string.Empty,
                    TypeId       = x.Model != null && x.Model.Type != null ? x.Model.Type.Id   : 0,
                    TypeCode     = x.Model != null && x.Model.Type != null ? x.Model.Type.Code : string.Empty,
                    TypeName     = x.Model != null && x.Model.Type != null ? x.Model.Type.Name : string.Empty,
                    BrandId      = x.Model != null && x.Model.Type != null && x.Model.Type.Brand != null ? x.Model.Type.Brand.Id   : 0,
                    BrandCode    = x.Model != null && x.Model.Type != null && x.Model.Type.Brand != null ? x.Model.Type.Brand.Code : string.Empty,
                    BrandName    = x.Model != null && x.Model.Type != null && x.Model.Type.Brand != null ? x.Model.Type.Brand.Name : string.Empty,
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
                    .ThenInclude(m => m.Type)
                        .ThenInclude(t => t.Brand)
                .Where(x => x.Id == id && x.DeletedAt == null)
                .Select(x => new ResMstStockDto
                {
                    Id = x.Id,
                    Color = x.Color,
                    PoliceNumber = x.PoliceNumber,
                    NewUsed = x.NewUsed,
                    Price = x.Price,
                    IsReady = x.IsReady,

                    ModelId = x.ModelId,
                    ModelCode = x.Model != null ? x.Model.Code : string.Empty,
                    ModelName = x.Model != null ? x.Model.Name : string.Empty,

                    TypeId = x.Model != null && x.Model.Type != null ? x.Model.Type.Id : 0,
                    TypeCode = x.Model != null && x.Model.Type != null ? x.Model.Type.Code : string.Empty,
                    TypeName = x.Model != null && x.Model.Type != null ? x.Model.Type.Name : string.Empty,

                    BrandId = x.Model != null && x.Model.Type != null && x.Model.Type.Brand != null ? x.Model.Type.Brand.Id : 0,
                    BrandCode = x.Model != null && x.Model.Type != null && x.Model.Type.Brand != null ? x.Model.Type.Brand.Code : string.Empty,
                    BrandName = x.Model != null && x.Model.Type != null && x.Model.Type.Brand != null ? x.Model.Type.Brand.Name : string.Empty,
                })
                .FirstOrDefaultAsync();

            return stock;
        }

        public async Task CreateMstStock(ReqCreateMstStockDto dto)
        {
            var stock = new MstStocks
            {
                ModelId = dto.ModelId,
                Color = dto.Color,
                PoliceNumber = dto.PoliceNumber,
                NewUsed = dto.NewUsed,
                Price = dto.Price,
                IsReady = dto.IsReady,
                CreatedAt = DateTime.Now,
                CreatedBy = dto.CreatedBy
            };

            _context.MstStocks.Add(stock);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateMstStock(ReqUpdateMstStockDto dto, int id)
        {
            var stock = await _context.MstStocks
                .FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);
            if (stock == null) return false;

            stock.ModelId = dto.ModelId;
            stock.Color = dto.Color;
            stock.PoliceNumber = dto.PoliceNumber;
            stock.NewUsed = dto.NewUsed;
            stock.Price = dto.Price;
            stock.IsReady = dto.IsReady;
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