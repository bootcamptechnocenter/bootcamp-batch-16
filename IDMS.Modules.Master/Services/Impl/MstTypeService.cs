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
    public class MstTypeService(AppDbContext context) : IMstTypeService
    {
        private readonly AppDbContext _context = context;
        public async Task<PagedResult<ResMstTypeDto>> GetMstTypes(ReqBaseParamDto dto)
        {

            var query = _context.MstTypes
                .Include(x => x.Brands)
                .AsQueryable();

            if (!string.IsNullOrEmpty(dto.Search))
            {
                var search = dto.Search.ToLower();
                query = query.Where(x =>
                    (x.Code != null && x.Code.ToLower().Contains(search)) ||
                    (x.Name != null && x.Name.ToLower().Contains(search))
                );
            }

            query = query.Where(x => x.DeletedAt == null);

            var totalItems = await query.CountAsync();

            
            var page = dto.Page <= 0 ? 1 : dto.Page;
            var limit = dto.Limit <= 0 ? 10 : dto.Limit; 
            var totalPages = (int)Math.Ceiling(totalItems / (double)limit);

           
            var items = await query
                .OrderBy(x => x.Id)
                .Skip((page - 1) * limit)
                .Take(limit)
                .Select(x => new ResMstTypeDto
                {
                    Id = x.Id,
                    MstBrandId = x.MstBrandId,
                    BrandName = x.Brands.Name, 
                    Code = x.Code,
                    Name = x.Name
                })
                .ToListAsync();

            
            return new PagedResult<ResMstTypeDto>
            {
                Items = items,
                Pagination = new Pagination
                {
                    CurrentPage = page,
                    Limit = limit,
                    TotalItems = totalItems,
                    TotalPages = totalPages
                }
            };
        }
        public async Task<ResMstTypeDto?> GetMstTypeById(int id)
        {
            return await _context.MstTypes
                .Include(x => x.Brands)
                .Where(x => x.Id == id && x.DeletedAt == null)
                .Select(x => new ResMstTypeDto
                {
                    Id = x.Id,
                    MstBrandId = x.MstBrandId,
                    BrandName = x.Brands.Name,
                    Code = x.Code,
                    Name = x.Name
                })
                .FirstOrDefaultAsync(); 
        }

        public async Task CreateMstType(ReqCreateMstType dto)
        {
            var type = new MstTypes
            {
                MstBrandId = dto.MstBrandId,
                Code = dto.Code,
                Name = dto.Name,
                IsActive = dto.IsActive,
                CreatedBy = dto.CreatedBy,
                CreatedAt = DateTime.Now
            };
            _context.MstTypes.Add(type);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateMstType(int id, ReqUpdateMstTypeDto dto)
        {
            var type = await _context.MstTypes.FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);
            if (type == null)
            {
                throw new Exception("Type not found");
            }
            type.MstBrandId = dto.MstBrandId;
            type.Code = dto.Code;
            type.Name = dto.Name;
            type.IsActive = dto.IsActive;
            type.UpdatedBy = dto.UpdatedBy;
            type.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
        }
        public async Task DeleteMstType(int id, string deletedBy)
        {
            var type = await _context.MstTypes.FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);

            if (type == null)
            {
                throw new Exception("Type not found");
            }

            type.DeletedAt = DateTime.Now;
            type.DeletedBy = deletedBy;

            type.IsActive = false;

            _context.MstTypes.Update(type);
            await _context.SaveChangesAsync();
        }
    }
}