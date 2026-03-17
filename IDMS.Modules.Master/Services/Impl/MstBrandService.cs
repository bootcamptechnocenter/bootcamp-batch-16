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

            var isExist = await _context.MstBrands.AnyAsync(x => 
                (x.Code.ToLower() == dto.Code.ToLower() || x.Name.ToLower() == dto.Name.ToLower()) 
                && x.DeletedAt == null);
            if (isExist) throw new ArgumentException("Brand Code or Name already exists.");

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
            if (brand == null) return false;

            var isExist = await _context.MstBrands.AnyAsync(x => 
                (x.Code.ToLower() == dto.Code.ToLower() || x.Name.ToLower() == dto.Name.ToLower()) 
                && x.Id != id && x.DeletedAt == null);

            if (isExist) throw new ArgumentException("Brand Code or Name already exists in another record.");
            
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
            var brand = await _context.MstBrands
            .FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);
            if (brand == null) return false;


            brand.DeletedAt = DateTime.Now;
            brand.DeletedBy = "system";

            await _context.SaveChangesAsync();
            return true;
        }
    }
}