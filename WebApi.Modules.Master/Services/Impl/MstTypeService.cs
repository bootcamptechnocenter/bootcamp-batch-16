using Microsoft.EntityFrameworkCore;
using WebApi.Infrastructure.Data;
using WebApi.Modules.Master.Dto.Response;
using WebApi.Shared.Entities;

namespace WebApi.Modules.Master.Services.Impl
{
    public class MstTypeService(AppDbContext context) : IMstTypeService
    {
        private readonly AppDbContext _context = context;

        public async Task<PagedResult<ResMstTypeDto>> GetMstTypes(ReqBaseParamDto dto)
        {
            var query = _context.MstTypes.AsQueryable();

            if (!string.IsNullOrEmpty(dto.Search))
            {
                var search = dto.Search.ToLower();
                query = query.Where(x =>
                    x.BrandId.ToString().ToLower().Contains(search) ||
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
                .Select(x => new ResMstTypeDto
                {
                    Id = x.Id,
                    BrandId = x.BrandId,
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

        public async Task<ResMstTypeDto> GetMstTypeById(int id)
        {
            var type = await _context.MstTypes
                .Where(x => x.Id == id && x.DeletedAt == null)
                .Select(x => new ResMstTypeDto
                {
                    Id = x.Id,
                    BrandId = x.BrandId,
                    Code = x.Code,
                    Name = x.Name
                })
                .FirstOrDefaultAsync() ?? throw new Exception("Type not found");

            return type;
        }
    }
}