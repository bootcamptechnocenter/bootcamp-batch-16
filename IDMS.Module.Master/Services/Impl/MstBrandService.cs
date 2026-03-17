using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Infrastructure.Data;
using IDMS.Module.Master.Dto.Request;
using IDMS.Module.Master.Dto.Response;
using IDMS.Shared.Domain.Entities;
using IDMS.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace IDMS.Module.Master.Services.Impl
{
    public class MstBrandService : IMstBrandService
    {
        private readonly AppDbContext _context;

        public MstBrandService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PageResult<RestMstBrandDto>> GetMstBrand(ReqBaseParamDto dto)
        {
            var query = _context.MstBrands.AsQueryable();

            if (!string.IsNullOrEmpty(dto.Search))
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
                .Select(x => new RestMstBrandDto
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                })
                .ToListAsync();

            return new PageResult<RestMstBrandDto>
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

        public async Task<RestMstBrandDto?> GetMstBrandById(int id)
        {
            var brand = await _context.MstBrands
                .Where(x => x.Id == id && x.DeletedAt == null)
                .Select(x => new RestMstBrandDto
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                })
                .FirstOrDefaultAsync();

            return brand;
        }

        public async Task CreateMstBrand(ReqCreateMstBrandDto dto)
        {
            var brand = new MstBrands
            {
                Code = dto.Code,
                Name = dto.Name,
                IsActive = true,
                CreatedAt = DateTime.Now,
                CreatedBy = "system"
            };

            _context.MstBrands.Add(brand);
            await _context.SaveChangesAsync();

        }

        public async Task UpdateNameMstBrand(int id, ReqUpdateNameMstBrandDto dto)
        {
            var brand = await _context.MstBrands
                .Where(x => x.Id == id && x.DeletedAt == null)
                .FirstOrDefaultAsync();

            if (brand == null)
            {
                throw new Exception("Brand not found");
            }

            brand.Name = dto.Name;
            brand.UpdatedAt = DateTime.Now;
            brand.UpdatedBy = "system";
            brand.CreatedAt = DateTime.SpecifyKind(brand.CreatedAt, DateTimeKind.Unspecified);

            _context.MstBrands.Update(brand);
            await _context.SaveChangesAsync();
        }

        // public async Task UpdateCodeMstBrand(int id, ReqUpdateNameMstBrandDto dto)
        // {
        //     var brand = await _context.MstBrands
        //         .Where(x => x.Id == id && x.DeletedAt == null)
        //         .FirstOrDefaultAsync();

        //     if (brand == null)
        //     {
        //         throw new Exception("Brand not found");
        //     }

        //     brand.Name = dto.Name;
        //     brand.UpdatedAt = DateTime.Now;
        //     brand.UpdatedBy = "system";
        //     brand.CreatedAt = DateTime.SpecifyKind(brand.CreatedAt, DateTimeKind.Unspecified);

        //     _context.MstBrands.Update(brand);
        //     await _context.SaveChangesAsync();
        // }
    }


}