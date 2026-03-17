using IDMS.Infrastructure.Data;
using IDMS.Modules.Master.Dto.Request;
using IDMS.Modules.Master.Dto.Response;
using IDMS.Shared.Domain.Entities;
using IDMS.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace IDMS.Modules.Master.Services.Impl
{
    public class MstModelService : IMstModelService
    {
        private readonly AppDbContext _context;

        public MstModelService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<ResMstModelDto>> GetMstModel(ReqBaseParamDto dto)
        {
            var query = _context.MstModels.AsQueryable();
            if (!string.IsNullOrWhiteSpace(dto.Search))
            {
                var search = dto.Search.ToLower();
                query = query.Where(x => x.Code.ToLower().Contains(search) || x.Name.ToLower().Contains(search));
            }

            query = query.Where(x => x.DeletedAt == null);

            var totalCount = await query.CountAsync();

            var page = dto.Page > 0 ? dto.Page : 1;
            var pageSize = dto.Limit > 0 ? dto.Limit : 10;
            var skip = (page - 1) * pageSize;
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var items = await query.OrderBy(x => x.Id)
                .Skip(skip)
                .Take(pageSize)
                .Select(x => new ResMstModelDto
                {
                    Id = x.Id,
                    TypeId = x.TypeId,
                    TypeName = x.Type != null ? x.Type.Name : string.Empty,
                    Code = x.Code,
                    Name = x.Name,
                    Year = x.Year,
                    IsActive = x.IsActive
                })
                .ToListAsync();

            return new PagedResult<ResMstModelDto>
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

        public async Task<ResMstModelDto?> GetMstModelById(int id)
        {
            var model = await _context.MstModels
                .Where(x => x.Id == id && x.DeletedAt == null)
                .Select(x => new ResMstModelDto
                {
                    Id = x.Id,
                    TypeId = x.TypeId,
                    TypeName = x.Type != null ? x.Type.Name : string.Empty,
                    Code = x.Code,
                    Name = x.Name,
                    Year = x.Year,
                    IsActive = x.IsActive
                })
                .FirstOrDefaultAsync();

            return model;
        }

        public async Task CreateMstModel(ReqCreateMstModelDto dto)
        {
            var model = new MstModels
            {
                TypeId = dto.TypeId,
                Code = dto.Code,
                Name = dto.Name,
                Year = dto.Year,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.Now,
                CreatedBy = "Admin"
            };

            _context.MstModels.Add(model);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateMstModel(int id, ReqUpdateMstModelDto dto)
        {
            var model = await _context.MstModels.FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);
            if (model == null) return false;

            model.TypeId = dto.TypeId;
            model.Code = dto.Code;
            model.Name = dto.Name;
            model.Year = dto.Year;
            model.IsActive = dto.IsActive;
            model.UpdatedAt = DateTime.Now;
            model.UpdatedBy = string.IsNullOrWhiteSpace(dto.UpdatedBy) ? "Admin" : dto.UpdatedBy;

            _context.MstModels.Update(model);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteMstModel(int id, string deletedBy)
        {
            var model = await _context.MstModels.FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);
            if (model == null) return false;

            model.DeletedAt = DateTime.Now;
            model.DeletedBy = string.IsNullOrWhiteSpace(deletedBy) ? "Admin" : deletedBy;

            _context.MstModels.Update(model);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
