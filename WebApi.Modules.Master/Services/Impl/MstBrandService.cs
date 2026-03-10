using Microsoft.EntityFrameworkCore;
using WebApi.Infrastructure.Data;
using WebApi.Modules.Master.Dto.Response;
using WebApi.Shared.Entities;

namespace WebApi.Modules.Master.Services.Impl
{
    public class MstBrandService(AppDbContext context) : IMstBrandService
    {
        private readonly AppDbContext _context = context;

        public async Task<PagedResult<ResMstBrandDto>> GetMstBrand(ReqBaseParamDto dto)
        {
            var query = _context.MstBrands.AsQueryable();

            if (!string.IsNullOrEmpty(dto.Search))
            {
                var search = dto.Search.ToLower();
                query = query.Where( x =>
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
                    Name = x.Name,
                })
                .ToListAsync();

            return new PagedResult<ResMstBrandDto>
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
    }
}