using Microsoft.EntityFrameworkCore;
using WebApi.Infrastructure.Data;
using WebApi.Modules.Master.Dto.Request;
using WebApi.Modules.Master.Dto.Response;
using WebApi.Shared.Domain.Entities;
using WebApi.Shared.Entities;

namespace WebApi.Modules.Master.Services.Impl
{
    public class MstModelService(AppDbContext context) : IMstModelService
    {
        private readonly AppDbContext _context = context;

        public async Task<PagedResult<ResMstModelDto>> GetMstModels(ReqBaseParamDto dto)
        {
            var query = _context.MstModels.AsQueryable();

            if (!string.IsNullOrEmpty(dto.Search))
            {
                var search = dto.Search.ToLower();
                query = query.Where(x =>
                    x.TypeId.ToString().ToLower().Contains(search) ||
                    (x.Code != null && x.Code.ToLower().Contains(search)) ||
                    (x.Name != null && x.Name.ToLower().Contains(search)) ||
                    x.Year.ToString().ToLower().Contains(search)
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
                .Select(x => new ResMstModelDto
                {
                    Id = x.Id,
                    TypeId = x.TypeId,
                    Code = x.Code,
                    Name = x.Name,
                    Year = x.Year,
                    IsActive = x.IsActive,
                })
                .ToListAsync();

            return new PagedResult<ResMstModelDto>
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

        public async Task<ResMstModelDto> GetMstModelById(int id)
        {
            var model = await _context.MstModels
                .Where(x => x.Id == id && x.DeletedAt == null)
                .Select(x => new ResMstModelDto
                {
                    Id = x.Id,
                    TypeId = x.TypeId,
                    Code = x.Code,
                    Name = x.Name,
                    Year = x.Year,
                    IsActive = x.IsActive,
                })
                .FirstOrDefaultAsync() ?? throw new Exception("Model not found");

            return model;
        }

        public async Task<ResMstModelDto> CreateMstModel(ReqMstModelDto dto)
        {
            var existingModel = await _context.MstModels
                .Where(x => x.Code == dto.Code && x.DeletedAt == null)
                .FirstOrDefaultAsync();

            if (existingModel != null) throw new Exception("Model code already exists");

            var model = new MstModels
            {
                TypeId = dto.TypeId,
                Code = dto.Code,
                Name = dto.Name,
                Year = dto.Year
            };

            _context.MstModels.Add(model);
            await _context.SaveChangesAsync();

            return new ResMstModelDto
            {
                Id = model.Id,
                TypeId = model.TypeId,
                Code = model.Code,
                Name = model.Name,
                Year = model.Year
            };
        }

        public async Task<ResMstModelDto> UpdateMstModel(int id, ReqMstModelUpdateDto dto)
        {
            var model = await _context.MstModels
                .Where(x => x.Id == id && x.DeletedAt == null)
                .FirstOrDefaultAsync() ?? throw new Exception("Model not found");

            if (!string.IsNullOrEmpty(dto.Code) && dto.Code != model.Code)
            {
                var existingModel = await _context.MstModels
                    .Where(x => x.Code == dto.Code && x.DeletedAt == null)
                    .FirstOrDefaultAsync();

                if (existingModel != null) throw new Exception("Model code already exists");
            }

            model.TypeId = dto.TypeId ?? model.TypeId;
            model.Code = dto.Code ?? model.Code;
            model.Name = dto.Name ?? model.Name;
            model.Year = dto.Year ?? model.Year;
            model.IsActive = dto.IsActive ?? model.IsActive;
            model.UpdatedAt = DateTime.Now;
            model.UpdatedBy = "System";

            await _context.SaveChangesAsync();

            return new ResMstModelDto
            {
                Id = model.Id,
                TypeId = model.TypeId,
                Code = model.Code,
                Name = model.Name,
                Year = model.Year
            };
        }

        public async Task DeleteMstModel(int id)
        {
            var model = await _context.MstModels
                .Where(x => x.Id == id && x.DeletedAt == null)
                .FirstOrDefaultAsync() ?? throw new Exception("Model not found");

            model.DeletedAt = DateTime.Now;
            model.DeletedBy = "System";

            await _context.SaveChangesAsync();
        }
    }
}