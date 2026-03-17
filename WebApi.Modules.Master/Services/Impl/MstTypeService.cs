using Microsoft.EntityFrameworkCore;
using WebApi.Infrastructure.Data;
using WebApi.Modules.Master.Dto.Request;
using WebApi.Modules.Master.Dto.Response;
using WebApi.Shared.Domain.Entities;
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
                    Name = x.Name,
                    IsActive = x.IsActive,
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
                    Name = x.Name,
                    IsActive = x.IsActive,
                })
                .FirstOrDefaultAsync() ?? throw new Exception("Type not found");

            return type;
        }

        public async Task<ResMstTypeDto> CreateMstType(ReqMstTypeDto dto)
        {
            var existingType = await _context.MstTypes
                .Where(x => x.Code == dto.Code && x.DeletedAt == null)
                .FirstOrDefaultAsync();

            if (existingType != null) throw new Exception("Type code already exists");

            var type = new MstTypes
            {
                BrandId = dto.BrandId,
                Code = dto.Code,
                Name = dto.Name,
                IsActive = dto.IsActive
            };

            _context.MstTypes.Add(type);
            await _context.SaveChangesAsync();

            return new ResMstTypeDto
            {
                Id = type.Id,
                BrandId = type.BrandId,
                Code = type.Code,
                Name = type.Name
            };
        }

        public async Task<ResMstTypeDto> UpdateMstType(int id, ReqMstTypeUpdateDto dto)
        {
            var type = await _context.MstTypes
                .Where(x => x.Id == id && x.DeletedAt == null)
                .FirstOrDefaultAsync() ?? throw new Exception("Type not found");

            if (!string.IsNullOrEmpty(dto.Code) && dto.Code != type.Code)
            {
                var existingType = await _context.MstTypes
                    .Where(x => x.Code == dto.Code && x.DeletedAt == null)
                    .FirstOrDefaultAsync();

                if (existingType != null) throw new Exception("Type code already exists");
            }

            type.BrandId = dto.BrandId ?? type.BrandId;
            type.Code = dto.Code ?? type.Code;
            type.Name = dto.Name ?? type.Name;
            type.IsActive = dto.IsActive ?? type.IsActive;
            type.UpdatedAt = DateTime.Now;
            type.UpdatedBy = "System";

            await _context.SaveChangesAsync();

            return new ResMstTypeDto
            {
                Id = type.Id,
                BrandId = type.BrandId,
                Code = type.Code,
                Name = type.Name
            };
        }

        public async Task ToggleActiveMstType(int id)
        {
            var type = await _context.MstTypes
                .Where(x => x.Id == id && x.DeletedAt == null)
                .FirstOrDefaultAsync() ?? throw new Exception("Type not found");

            type.IsActive = !type.IsActive;
            type.UpdatedAt = DateTime.Now;
            type.UpdatedBy = "System";

            await _context.SaveChangesAsync();
        }

        public async Task DeleteMstType(int id)
        {
            var type = await _context.MstTypes
                .Where(x => x.Id == id && x.DeletedAt == null)
                .FirstOrDefaultAsync() ?? throw new Exception("Type not found");

            type.DeletedAt = DateTime.Now;
            type.DeletedBy = "System";

            await _context.SaveChangesAsync();
        }
    }
}