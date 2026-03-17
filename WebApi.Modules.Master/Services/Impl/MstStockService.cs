using Microsoft.EntityFrameworkCore;
using WebApi.Infrastructure.Data;
using WebApi.Modules.Master.Dto.Request;
using WebApi.Modules.Master.Dto.Response;
using WebApi.Shared.Domain.Entities;
using WebApi.Shared.Entities;

namespace WebApi.Modules.Master.Services.Impl
{
    public class MstStockService(AppDbContext context) : IMstStockService
    {
        private readonly AppDbContext _context = context;

        public async Task<PagedResult<ResMstStockDto>> GetMstStocks(ReqBaseParamDto dto)
        {
            var query = from stock in _context.MstStocks
                        join model in _context.MstModels on stock.ModelId equals model.Id
                        join type in _context.MstTypes on model.TypeId equals type.Id
                        join brand in _context.MstBrands on type.BrandId equals brand.Id
                        where stock.DeletedAt == null
                        select new { stock, model, type, brand };

            if (!string.IsNullOrEmpty(dto.Search))
            {
                var search = dto.Search.ToLower();
                query = query.Where(x =>
                    (x.brand.Name != null && x.brand.Name.ToLower().Contains(search)) ||
                    (x.type.Name != null && x.type.Name.ToLower().Contains(search)) ||
                    (x.model.Name != null && x.model.Name.ToLower().Contains(search))
                );
            }

            var totalCount = await query.CountAsync();

            var page = dto.Page > 0 ? dto.Page : 1;
            var pageSize = dto.Limit > 0 ? dto.Limit : 10;
            var skip = (page - 1) * pageSize;
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var items = await query
                .OrderBy(x => x.stock.Id)
                .Skip(skip)
                .Take(pageSize)
                .Select(x => new ResMstStockDto
                {
                    Id = x.stock.Id,
                    ModelId = x.stock.ModelId,
                    BrandName = x.brand.Name,
                    TypeName = x.type.Name,
                    ModelName = x.model.Name,
                    Name = string.Join(" ", new[] { x.brand.Name, x.type.Name, x.model.Name }.Where(n => !string.IsNullOrWhiteSpace(n))),
                    Stock = x.stock.Stock,
                    Price = x.stock.Price
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

        public async Task<ResMstStockDto> GetMstStockById(int id)
        {
            var result = await (from stock in _context.MstStocks
                                join model in _context.MstModels on stock.ModelId equals model.Id
                                join type in _context.MstTypes on model.TypeId equals type.Id
                                join brand in _context.MstBrands on type.BrandId equals brand.Id
                                where stock.Id == id && stock.DeletedAt == null
                                select new ResMstStockDto
                                {
                                    Id = stock.Id,
                                    ModelId = stock.ModelId,
                                    BrandName = brand.Name,
                                    TypeName = type.Name,
                                    ModelName = model.Name,
                                    Name = string.Join(" ", new[] { brand.Name, type.Name, model.Name }.Where(n => !string.IsNullOrWhiteSpace(n))),
                                    Stock = stock.Stock,
                                    Price = stock.Price
                                })
                                .FirstOrDefaultAsync() ?? throw new Exception("Stock not found");

            return result;
        }

        public async Task<ResMstStockDto> CreateMstStock(ReqMstStockDto dto)
        {
            var model = await _context.MstModels
                .Where(x => x.Id == dto.ModelId && x.DeletedAt == null)
                .FirstOrDefaultAsync() ?? throw new Exception("Model not found");

            var existingStock = await _context.MstStocks
                .AnyAsync(x => x.ModelId == dto.ModelId && x.DeletedAt == null);

            if (existingStock) throw new Exception("Stock for this model already exists");

            var stock = new MstStocks
            {
                ModelId = dto.ModelId,
                Stock = dto.Stock,
                Price = dto.Price,
                CreatedAt = DateTime.Now,
                CreatedBy = "System"
            };

            _context.MstStocks.Add(stock);
            await _context.SaveChangesAsync();

            return await GetMstStockById(stock.Id);
        }

        public async Task<ResMstStockDto> UpdateMstStock(int id, ReqMstStockUpdateDto dto)
        {
            var stock = await _context.MstStocks
                .Where(x => x.Id == id && x.DeletedAt == null)
                .FirstOrDefaultAsync() ?? throw new Exception("Stock not found");

            if (dto.ModelId.HasValue && dto.ModelId.Value != stock.ModelId)
            {
                var modelExists = await _context.MstModels
                    .AnyAsync(x => x.Id == dto.ModelId.Value && x.DeletedAt == null);

                if (!modelExists) throw new Exception("Model not found");

                var existingStock = await _context.MstStocks
                    .AnyAsync(x => x.ModelId == dto.ModelId.Value && x.DeletedAt == null && x.Id != id);

                if (existingStock) throw new Exception("Stock for this model already exists");

                stock.ModelId = dto.ModelId.Value;
            }

            if (dto.Stock.HasValue) stock.Stock = dto.Stock.Value;
            if (dto.Price.HasValue) stock.Price = dto.Price.Value;
            stock.UpdatedAt = DateTime.Now;
            stock.UpdatedBy = "System";

            await _context.SaveChangesAsync();

            return await GetMstStockById(stock.Id);
        }

        public async Task DeleteMstStock(int id)
        {
            var stock = await _context.MstStocks
                .Where(x => x.Id == id && x.DeletedAt == null)
                .FirstOrDefaultAsync() ?? throw new Exception("Stock not found");

            stock.DeletedAt = DateTime.Now;
            stock.DeletedBy = "System";

            await _context.SaveChangesAsync();
        }
    }
}
