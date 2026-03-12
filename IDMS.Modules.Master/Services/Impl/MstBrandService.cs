using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
                query = query.Where(x => x.Code.ToLower().Contains(search) || x.Name.ToLower().Contains(search));
            }

            query = query.Where(x => x.DeletedAt == null);

            var totalCount = await query.CountAsync();

            var page = dto.Page > 0 ? dto.Page : 1;
            var pageSize = dto.Limit > 0 ? dto.Limit : 10;
            var skip = (page - 1) * pageSize;
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var items = await query.OrderBy(x => x.Id)
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
                Pagination = new Pagination
                {
                    CurrentPage = page,
                    Limit = pageSize,
                    TotalItems = totalCount,
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

        public async Task CreateMstBrand(ReqCreateMstBrancDto dto)
        {
            var brand = new MstBrands
            {
                Code = dto.Code,
                Name = dto.Name,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.Now,
                CreatedBy = "Admin"
            };
            _context.MstBrands.Add(brand);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateMstBrand(int id, ReqUpdateMstBrancDto dto)
        {
            var brand = _context.MstBrands.FirstOrDefault(x => x.Id == id && x.DeletedAt == null);
            if (brand == null) return false;

            brand.Code = dto.Code;
            brand.Name = dto.Name;
            brand.IsActive = dto.IsActive;
            brand.UpdatedAt = DateTime.Now;
            brand.UpdatedBy = string.IsNullOrWhiteSpace(dto.UpdatedBy) ? "Admin" : dto.UpdatedBy;

            _context.MstBrands.Update(brand);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteMstBrand(int id, string deletedBy)
        {
            var brand = await _context.MstBrands.FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);
            if (brand == null) return false;

            brand.DeletedAt = DateTime.Now;
            brand.DeletedBy = string.IsNullOrWhiteSpace(deletedBy) ? "Admin" : deletedBy;

            _context.MstBrands.Update(brand);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}