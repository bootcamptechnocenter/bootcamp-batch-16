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
    public class MstBrandService: IMstBrandService
    {
        private readonly AppDbContext _context;
        public MstBrandService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<PagedResult<ResMstBrandDto>> GetMstBrand(ReqBaseParamDto dto)
        {
            var query = _context.MstBrands.AsQueryable();

            if(!string.IsNullOrWhiteSpace(dto.Search))
            {
                var search = dto.Search.Trim().ToLower();
                query = query.Where(x => x.Code.ToLower().Contains(search) || 
                x.Name.ToLower().Contains(search));
            }

            query = query.Where(x => x.DeletedAt == null);

            var totalCount = await query.CountAsync();

            var page = dto.Page > 0 ? dto.Page : 1;
            var pageSize = dto.Limit > 0 ? dto.Limit : 10;
            var skip = (page - 1)*pageSize;
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var items = await query
                .OrderBy(x => x.Id)
                .Skip(skip)
                .Take(pageSize)
                .Select(x => new ResMstBrandDto
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    IsActive = x.IsActive
                })
                .ToListAsync();
                Console.WriteLine("Total Pages: {0}, Items Count: {1}", totalPages, items.Count);
                Console.WriteLine("Current Page: {0}, Limit: {1}", page, pageSize);
            return new PagedResult<ResMstBrandDto>{
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


        public async Task<ResMstBrandDto> GetMstBrandById(int id)
        {            
            var brand = _context.MstBrands.AsQueryable();

            brand = brand.Where(x => x.Id == id && x.DeletedAt == null);

            var items = await brand
                .OrderBy(x => x.Id)
                .Select(x => new ResMstBrandDto
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    IsActive = x.IsActive
                })
                .ToListAsync();
            return items.FirstOrDefault();
        }

        public async Task CreateMstBrand(ReqCreateMstBrandDto dto)
        {
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

        public async Task UpdateMstBrand(ReqUpdateMstBrandDto dto, int id)
        {
            var brand = await _context.MstBrands.FindAsync(id);
            if(brand == null || brand.DeletedAt != null)
            {
                throw new Exception("Brand not found");
            }

            brand.Code = dto.Code;
            brand.Name = dto.Name;
            brand.IsActive = dto.IsActive;
            brand.UpdatedAt = DateTime.Now;
            brand.UpdatedBy = dto.UpdatedBy;

            _context.MstBrands.Update(brand);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteMstBrand(int id)
        {
            var brand = await _context.MstBrands
            .FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);
            if(brand == null || brand.DeletedAt != null)
            {
                return false;
            }

            brand.DeletedAt = DateTime.Now;
            brand.DeletedBy = "Admin";
            await _context.SaveChangesAsync();
            return true;
        }
    }
}