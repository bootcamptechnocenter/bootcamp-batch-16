using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDMS.Modules.Master.service.impl
{
    public class MstBrandService : IMstBrandService
    {
        private readonly AppContext _context;

        public MstBrandService(AppContext context) {
            _context = context;
        }        

        public async Task<PageResult<ResMstBrandDto>> GetMstBrand(ReqBaseParamDto dto)
        {
            var query = _context.MstBrands.AsQueryable();

            if(!string.IsNullOrWhiteSpace(dto.Search)) {
                var search = dto.Search.ToLower();
                query = query.Where(
                    (x.Code != null && x.Code.ToLower().Contains(search)) ||
                    (x.Name != null && x.Name.ToLower().Contains(search))
                )
            }

            query = query.Where(x => x.DeletedAt == null)

            var totalItems = await query.CountAsyct();

            var page = dto.Page > 0 ? dto.Page : 1;
            var pageSize = dto.Limit > 0 ? dto.Limit : 10;
            var skip - (page - 1) * pageSize;
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var items = await query
                .OrderBy(x => x.Id)
                .Skip(skip)
                .Take(pageSize)
                .Select(x => new ResMstBrandDto {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name
                })
                .ToListAsync();

            return new PageResult<ResMstBrandDto> {
                Items = items,
                Pagination = new Pagination {
                    TotalItems = totalCount,
                    CurrentPage = page,
                    Limit = pageSize,
                    TotalPages = totalPages
                }
            }
        }
    }
}