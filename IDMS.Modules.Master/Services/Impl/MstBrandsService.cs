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
    public class MstBrandsService : IMstBrandsService
    {
        private readonly AppDbContext _dbContext;

        public MstBrandsService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedResult<ResMstBrandsDto>> GetMstBrands(ReqBaseParamDto dto)
        {
            var query = _dbContext.MstBrands.AsQueryable();

            if (!string.IsNullOrWhiteSpace(dto.Search))
            {
                var search = dto.Search.ToLower();
                query = query.Where(x => x.Code != null && x.Code.ToLower().Contains(search) ||
                                         x.Name != null && x.Name.ToLower().Contains(search));
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
                .Select(x => new ResMstBrandsDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code
                })
                .ToListAsync();

            return new PagedResult<ResMstBrandsDto>
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

        public async Task<ResMstBrandsDto?> GetMstBrandsById(int id)
        {
            var brand = await _dbContext.MstBrands
            .Where(x => x.Id == id && x.DeletedAt == null)
            .Select(x => new ResMstBrandsDto
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
            var newBrand = new MstBrands
            {
                Code = dto.Code,
                Name = dto.Name,
                IsActive = dto.IsActive,
                CreatedBy = "SYSTEM"
            };

            _dbContext.MstBrands.Add(newBrand);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> UpdateMstBrand(int id, ReqUpdateMstBrandDto dto)
        {
            var brand = await _dbContext.MstBrands.FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null)
                        ?? throw new InvalidOperationException("Data Not Found!");

            if (!string.IsNullOrEmpty(dto.Code)) brand.Code = dto.Code;
            if (!string.IsNullOrEmpty(dto.Name)) brand.Name = dto.Name;
            if (dto.IsActive.HasValue) brand.IsActive = dto.IsActive.Value;
            if (!string.IsNullOrEmpty(dto.UpdatedBy)) brand.UpdatedBy = dto.UpdatedBy;
            brand.UpdatedAt = DateTime.Now;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteMstBrand(int id)
        {
            var brand = await _dbContext.MstBrands
                .FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);

            if (brand == null) return false;

            brand.DeletedAt = DateTime.Now;
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}