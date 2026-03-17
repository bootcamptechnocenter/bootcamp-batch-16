using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Infrastructure.Data;
using IDMS.Module.Master.Dto.Response;
using IDMS.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace IDMS.Module.Master.Services.Impl
{
    public class MstTypeService : IMstTypeService
    {
        private readonly AppDbContext _context;

        public MstTypeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PageResult<RestMstTypeDto>> GetMstType(ReqBaseParamDto dto)
        {
            var query = _context.MstTypes.Include(x => x.Brand).AsQueryable();

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
                .Select(x => new RestMstTypeDto
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    MstBrandId = x.Brand.Id,
                    MstBrandName = x.Brand.Name,
                    Brand = x.Brand
                })
                .ToListAsync();

            return new PageResult<RestMstTypeDto>
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
    }
}