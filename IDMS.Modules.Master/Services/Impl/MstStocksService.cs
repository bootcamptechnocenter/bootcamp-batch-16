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
    public class MstStocksService : IMstStocksService
    {
        private readonly AppDbContext _dbContext;

        public MstStocksService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CreateStock(ReqCreateStockDto dto)
        {
            var isModelExist = await _dbContext.MstModels.AnyAsync(x => x.Id == dto.ModelId && x.DeletedAt == null);
            if (!isModelExist)
            {
                return false;
            }

            var stock = new MstStocks
            {
                ModelId = dto.ModelId,
                Quantity = dto.Quantity,
                Price = dto.Price,
                CreatedBy = dto.CreatedBy,
                CreatedAt = DateTime.Now
            };

            _dbContext.MstStocks.Add(stock);
            var result = await _dbContext.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteStock(int id)
        {
            var stock = await _dbContext.MstStocks.FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);
            if (stock == null) return false;

            stock.DeletedAt = DateTime.Now;
            stock.DeletedBy = "System";
            _dbContext.MstStocks.Update(stock);
            var result = await _dbContext.SaveChangesAsync();
            return result > 0;
        }

        public async Task<PagedResult<ResMstStocksDto>> GetAllStocks(ReqBaseParamDto dto)
        {
            var query = _dbContext.MstStocks.Include(x => x.Model)
                                            .ThenInclude(x => x.Type)
                                            .ThenInclude(x => x.Brand)
                                            .AsQueryable();

            if (!string.IsNullOrWhiteSpace(dto.Search))
            {
                var search = dto.Search.ToLower();
                query = query.Where(x => x.Model.Name.ToLower().Contains(search) ||
                                         x.Model.Type.Name.ToLower().Contains(search) ||
                                         x.Model.Type.Brand.Name.ToLower().Contains(search));
            }

            query = query.Where(x => x.DeletedAt == null);

            var totalCount = await query.CountAsync();

            var items = await query.Skip((dto.Page - 1) * dto.Limit)
                                   .Take(dto.Limit)
                                   .Select(x => new ResMstStocksDto
                                   {
                                       Id = x.Id,
                                       BrandId = x.Model.Type.Brand.Id,
                                       TypeId = x.Model.Type.Id,
                                       ModelId = x.Model.Id,
                                       ModelName = x.Model.Name,
                                       TypeName = x.Model.Type.Name,
                                       BrandName = x.Model.Type.Brand.Name,
                                       Quantity = x.Quantity,
                                       Price = x.Price
                                   })
                                   .ToListAsync();

            return new PagedResult<ResMstStocksDto>
            {
                Items = items,
                Pagination = new Pagination
                {
                    CurrentPage = dto.Page,
                    Limit = dto.Limit,
                    TotalItems = totalCount,
                    TotalPages = (int)Math.Ceiling((double)totalCount / dto.Limit)
                }
            };

        }

        public async Task<ResMstStocksDto?> GetStockById(int id)
        {
            var stock = await _dbContext.MstStocks.Include(x => x.Model)
                                                  .ThenInclude(x => x.Type)
                                                  .ThenInclude(x => x.Brand)
                                                  .FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);

            if (stock == null)
            {
                return null;
            }

            return new ResMstStocksDto
            {
                Id = stock.Id,
                BrandId = stock.Model.Type.Brand.Id,
                TypeId = stock.Model.Type.Id,
                ModelId = stock.Model.Id,
                ModelName = stock.Model.Name,
                TypeName = stock.Model.Type.Name,
                BrandName = stock.Model.Type.Brand.Name,
                Quantity = stock.Quantity,
                Price = stock.Price
            };
        }

        public async Task<bool> UpdateStock(int id, ReqUpdateStockDto dto)
        {
            var stock = await _dbContext.MstStocks.FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);
            var isModelExist = await _dbContext.MstModels.AnyAsync(x => x.Id == dto.ModelId && x.DeletedAt == null);
            var isTypeExist = await _dbContext.MstTypes.AnyAsync(x => x.Id == dto.TypeId && x.DeletedAt == null);
            var isBrandExist = await _dbContext.MstBrands.AnyAsync(x => x.Id == dto.BrandId && x.DeletedAt == null);
            if (stock == null || !isModelExist || !isTypeExist || !isBrandExist)
            {
                throw new InvalidOperationException(stock == null ? $"Stock with Id of {id} Not Found" :
                    (!isModelExist ? $"Model with Id of {dto.ModelId} Not Found" :
                    (!isTypeExist ? $"Type with Id of {dto.TypeId} Not Found" :
                    $"Brand with Id of {dto.BrandId} Not Found")));
            }

            stock.ModelId = dto.ModelId;
            stock.Quantity = dto.Quantity;
            stock.Price = dto.Price;
            stock.UpdatedBy = dto.UpdatedBy;
            stock.UpdatedAt = DateTime.Now;

            _dbContext.MstStocks.Update(stock);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}