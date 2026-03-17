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
    public class MstTypeService : IMstTypeService
    {
        private readonly AppDbContext _context;

        public MstTypeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<ResMstTypeDto>> GetMstType(ReqGetTypeDto dto)
        {
            var query = _context.MstTypes
                .Include(x => x.Brand)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(dto.Search))
            {
                var search = dto.Search.ToLower();
                query = query.Where(x =>
                    (x.Code != null && x.Code.ToLower().Contains(search)) ||
                    (x.Name != null && x.Name.ToLower().Contains(search)) ||
                    (x.Brand != null && x.Brand.Code.ToLower().Contains(search)) ||
                    (x.Brand != null && x.Brand.Name.ToLower().Contains(search))
                );
            }

            if (dto.BrandId.HasValue)
            {
                query = query.Where(x => x.BrandId == dto.BrandId.Value);
            }

            query = query.Where(x => x.DeletedAt == null);

            var totalCount = await query.CountAsync();

            var page = dto.Page > 0 ? dto.Page : 1;
            var pageSize = dto.Limit > 0 ? dto.Limit : 10;
            var skip = (page - 1) * pageSize;
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var items = await query
                .OrderBy(x => x.BrandId)
                .ThenBy(x => x.Id)
                .Skip(skip)
                .Take(pageSize)
                .Select(x => new ResMstTypeDto
                {
                    Id = x.Id,
                    BrandId = x.BrandId,
                    BrandCode = x.Brand != null ? x.Brand.Code : string.Empty,
                    BrandName = x.Brand != null ? x.Brand.Name : string.Empty,
                    Code = x.Code,
                    Name = x.Name
                })
                .ToListAsync();

            return new PagedResult<ResMstTypeDto>
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

        public async Task<ResMstTypeDto?> GetMstTypeById(int id)
        {
            var type = await _context.MstTypes
                .Include(x => x.Brand)
                .Where(x => x.Id == id && x.DeletedAt == null)
                .Select(x => new ResMstTypeDto
                {
                    Id = x.Id,
                    BrandId = x.BrandId,
                    BrandCode = x.Brand != null ? x.Brand.Code : string.Empty,
                    BrandName = x.Brand != null ? x.Brand.Name : string.Empty,
                    Code = x.Code,
                    Name = x.Name
                })
                .FirstOrDefaultAsync();

            return type;
        }

        public async Task CreateMstType(ReqCreateMstTypeDto dto)
        {
            var type = new MstTypes
            {
                BrandId = dto.BrandId,
                Code = dto.Code,
                Name = dto.Name,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.Now,
                CreatedBy = dto.CreatedBy
            };

            _context.MstTypes.Add(type);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateMstType(ReqUpdateMstTypeDto dto, int id)
        {
            var type = await _context.MstTypes
                .FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);
            if (type == null) return false;

            type.BrandId = dto.BrandId;
            type.Code = dto.Code;
            type.Name = dto.Name;
            type.IsActive = dto.IsActive;
            type.UpdatedAt = DateTime.Now;
            type.UpdatedBy = dto.UpdatedBy;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteMstType(int id)
        {
            var type = await _context.MstTypes
                .FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);
            if (type == null) return false;

            type.DeletedAt = DateTime.Now;
            type.DeletedBy = "system";

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
