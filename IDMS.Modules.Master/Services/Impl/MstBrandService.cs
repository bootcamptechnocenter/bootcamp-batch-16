using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Infrastructure.Data;
using IDMS.Modules.Master.Dto.Response;
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
                    Name = x.Name
                })
                .ToListAsync();
                // Console.WriteLine("Total Pages: {0}, Items Count: {1}", totalPages, items.Count);
                // Console.WriteLine("Current Page: {0}, Limit: {1}", page, pageSize);
            return new PagedResult<ResMstBrandDto>{
                Items = items,
                Pagination = new Pagination
                {
                    TotalItems = items.Count,
                    CurrentPage = page,
                    Limit = pageSize,
                    TotalPages = totalPages
                }

            };
        }
    }
}