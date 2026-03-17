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
    public class MstBrandService : IMstBrandService
    {
        private readonly AppDbContext _context;
        public MstBrandService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<PagedResult<ResMstBrandDto>> GetMstBrand(ReqBaseParamDto dto)
        {
            var query = _context.MstBrands.AsQueryable();

            if (!string.IsNullOrWhiteSpace(dto.Search))
            {
                var search = dto.Search.ToLower();
                query = query.Where(x =>
                    (x.Code != null && x.Code.ToLower().Contains(search)) ||
                    (x.Name != null && x.Name.ToLower().Contains(search))
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
                .Select(x => new ResMstBrandDto
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name
                })
                .ToListAsync();

            return new PagedResult<ResMstBrandDto>
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

        public async Task<ResMstBrandDto?> GetMstBrandById(int id)
        {
            var brand = await _context.MstBrands
                .Where(x => x.Id == id && x.DeletedAt == null)
                .Select(x => new ResMstBrandDto
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name
                })
                .FirstOrDefaultAsync();

            return brand;
        }

        public async Task CreateMstBrand(ReqCreateMstBrandDto dto)
        {
            var isDuplicate = await _context.MstBrands
                .AnyAsync(m =>
                    m.Code.ToLower() == dto.Code.ToLower());

            if (isDuplicate)
            {
                throw new Exception($"Brand '{dto.Code}' is already exists!");
            }
            var brand = new MstBrands
            {
                Code = dto.Code,
                Name = dto.Name,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.Now,
                CreatedBy = dto.CreatedBy
            };

            _context.MstBrands.Add(brand);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateMstBrand(ReqUpdateMstBrandDto dto, int id)
        {
            var brand = _context.MstBrands.FirstOrDefault(x => x.Id == id && x.DeletedAt == null);
            if (brand == null)
            {
                throw new Exception("Brand is not found.");
            }

            var isDuplicate = await _context.MstBrands
                .AnyAsync(m =>
                    m.Code.ToLower() == dto.Code.ToLower() &&
                    m.Id != id &&
                    m.DeletedAt == null);

            if (isDuplicate)
            {
                throw new Exception($"Brand '{dto.Code}' is already exists!");
            }

            brand.Code = dto.Code;
            brand.Name = dto.Name;
            brand.IsActive = dto.IsActive;
            brand.UpdatedAt = DateTime.Now;
            brand.UpdatedBy = dto.UpdatedBy;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteMstBrand(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var now = DateTime.Now;
                var deletedBy = "system";

                var typeIds = await _context.MstTypes
                    .Where(t => t.BrandId == id && t.DeletedAt == null)
                    .Select(t => t.Id)
                    .ToListAsync();

                if (typeIds.Any())
                {
                    var modelIds = await _context.MstModels
                        .Where(m => typeIds.Contains(m.TypeId) && m.DeletedAt == null)
                        .Select(m => m.Id)
                        .ToListAsync();

                    if (modelIds.Any())
                    {
                        await _context.MstStocks
                            .Where(s => modelIds.Contains(s.ModelId) && s.DeletedAt == null)
                            .ExecuteUpdateAsync(setters => setters
                                .SetProperty(s => s.DeletedAt, now)
                                .SetProperty(s => s.DeletedBy, deletedBy));

                        await _context.MstModels
                            .Where(m => modelIds.Contains(m.Id) && m.DeletedAt == null)
                            .ExecuteUpdateAsync(setters => setters
                                .SetProperty(m => m.DeletedAt, now)
                                .SetProperty(m => m.DeletedBy, deletedBy));
                    }

                    await _context.MstTypes
                        .Where(t => typeIds.Contains(t.Id) && t.DeletedAt == null)
                        .ExecuteUpdateAsync(setters => setters
                            .SetProperty(t => t.DeletedAt, now)
                            .SetProperty(t => t.DeletedBy, deletedBy));
                }

                var brand = await _context.MstBrands
                    .FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);

                if (brand == null) return false;

                brand.DeletedAt = now;
                brand.DeletedBy = deletedBy;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}