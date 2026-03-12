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
        public async Task<PagedResult<ResMstTypeDto>> GetMstTypes(ReqBaseParamDto dto)
        {
            var query = _context.MstTypes.AsQueryable();

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
                .Select(x => new ResMstTypeDto
                {
                    Id = x.Id,
                    BrandId = x.BrandId,
                    BrandName = x.Brand != null ? x.Brand.Name : "",
                    Code = x.Code,
                    Name = x.Name,
                    IsActive = x.IsActive
                })
                .ToListAsync();
                Console.WriteLine("Total Pages: {0}, Items Count: {1}", totalPages, items.Count);
                Console.WriteLine("Current Page: {0}, Limit: {1}", page, pageSize);
            return new PagedResult<ResMstTypeDto>{
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


        public async Task<ResMstTypeDto> GetMstTypeById(int id)
        {            
            var type = _context.MstTypes.AsQueryable();

            type = type.Where(x => x.Id == id && x.DeletedAt == null);

            var items = await type
                .OrderBy(x => x.Id)
                .Select(x => new ResMstTypeDto
                {
                    Id = x.Id,
                    BrandId = x.BrandId,
                    Code = x.Code,
                    Name = x.Name,
                    IsActive = x.IsActive
                })
                .ToListAsync();
            return items.FirstOrDefault();
        }

        public async Task CreateMstType(ReqCreateMstTypeDto dto)
        {
            var type = new MstTypes
            {
                Code = dto.Code,
                Name = dto.Name,
                BrandId = dto.BrandId,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.Now,
                CreatedBy = dto.CreatedBy
            };
            _context.MstTypes.Add(type);
            await _context.SaveChangesAsync();

        }

        public async Task UpdateMstType(int id, ReqUpdateMstTypeDto dto)
        {
            var type = await _context.MstTypes.FindAsync(id);
            if(type == null || type.DeletedAt != null)
            {
                throw new Exception("Type not found");
            }

            type.Code = dto.Code;
            type.Name = dto.Name;
            type.IsActive = dto.IsActive;
            type.UpdatedAt = DateTime.Now;
            type.UpdatedBy = dto.UpdatedBy;

            _context.MstTypes.Update(type);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteMstType(int id)
        {
            var type = await _context.MstTypes
            .FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);
            if(type == null || type.DeletedAt != null)
            {
                return false;
            }

            type.DeletedAt = DateTime.Now;
            type.DeletedBy = "Admin";
            await _context.SaveChangesAsync();
            return true;
        }
    }
}