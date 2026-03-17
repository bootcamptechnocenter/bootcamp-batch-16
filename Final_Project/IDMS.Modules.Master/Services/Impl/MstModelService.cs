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
    public class MstModelService : IMstModelService
    {
        private readonly AppDbContext _context;
        public MstModelService(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateMstModel(ReqCreateMstModelDto dto)
        {
            var targetType = await _context.MstTypes
            .Include(t => t.Brand)
            .FirstOrDefaultAsync(t => t.Id == dto.TypeId && t.DeletedAt == null);

            if (targetType == null)
            {
                throw new Exception("Type is not found.");
            }

            var isDuplicate = await _context.MstModels
                .AnyAsync(m =>
                    m.Code.ToLower() == dto.Code.ToLower() &&
                    m.Type.Code.ToLower() == targetType.Code.ToLower() &&
                    m.Type.Brand.Code.ToLower() == targetType.Brand.Code.ToLower() &&
                    m.DeletedAt == null);

            if (isDuplicate)
            {
                throw new Exception($"Brand '{targetType.Brand.Code}', Type '{targetType.Code}', and Model '{dto.Code}' is already exists!");
            }

            var model = new MstModels
            {
                TypeId = dto.TypeId,
                Code = dto.Code,
                Name = dto.Name,
                IsActive = dto.IsActive,
                Year = dto.Year,
                CreatedAt = DateTime.Now,
                CreatedBy = dto.CreatedBy
            };

            _context.MstModels.Add(model);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteMstModel(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var now = DateTime.Now;
                var deletedBy = "System";

                await _context.MstStocks
                    .Where(s => s.ModelId == id && s.DeletedAt == null)
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(s => s.DeletedAt, now)
                        .SetProperty(s => s.DeletedBy, deletedBy));

                var model = await _context.MstModels
                    .FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);

                if (model == null) return false;

                model.DeletedAt = now;
                model.DeletedBy = deletedBy;

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<PagedResult<ResMstModelDto>> GetMstModel(ReqGetModelDto dto)
        {
            var query = _context.MstModels
                .Include(x => x.Type)
                .ThenInclude(x => x.Brand)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(dto.Search))
            {
                var search = dto.Search.ToLower();
                query = query.Where(x =>
                    (x.Code != null && x.Code.ToLower().Contains(search)) ||
                    (x.Name != null && x.Name.ToLower().Contains(search)) ||
                    (x.Type != null && x.Type.Name.ToLower().Contains(search)) ||
                    (x.Type != null && x.Type.Name.ToLower().Contains(search)) ||
                    (x.Type != null && x.Type.Brand != null && x.Type.Brand.Name.ToLower().Contains(search)) ||
                    (x.Type != null && x.Type.Brand != null && x.Type.Brand.Code.ToLower().Contains(search))
                );
            }

            if (dto.IsDoNotHaveStock == true)
            {
                query = query.Where(m => !_context.MstStocks.Any(s => 
                    s.ModelId == m.Id && 
                    s.DeletedAt == null
                ));
            }
            query = query.Where(x => x.DeletedAt == null);

            var totalCount = await query.CountAsync();

            var page = dto.Page > 0 ? dto.Page : 1;
            var pageSize = dto.Limit > 0 ? dto.Limit : 10;
            var skip = (page - 1) * pageSize;
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var items = await query
                .OrderBy(x => x.TypeId)
                .ThenBy(x => x.Id)
                .Skip(skip)
                .Take(pageSize)
                .Select(x => new ResMstModelDto
                {
                    TypeCode = x.Type != null ? x.Type.Code : string.Empty,
                    TypeId = x.TypeId,
                    TypeName = x.Type != null ? x.Type.Name : string.Empty,
                    Id = x.Id,
                    Year = x.Year,
                    BrandCode = (x.Type != null && x.Type.Brand != null) ? x.Type.Brand.Code : string.Empty,
                    BrandName = (x.Type != null && x.Type.Brand != null) ? x.Type.Brand.Name : string.Empty,
                    Code = x.Code,
                    Name = x.Name
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

        public async Task<ResMstModelDto?> GetMstModelById(int id)
        {
            var type = await _context.MstModels
                .Include(x => x.Type)
                .ThenInclude(x => x.Brand)
                .Where(x => x.Id == id && x.DeletedAt == null)
                .Select(x => new ResMstModelDto
                {
                    TypeCode = x.Type != null ? x.Type.Code : string.Empty,
                    TypeId = x.TypeId,
                    TypeName = x.Type != null ? x.Type.Name : string.Empty,
                    Id = x.Id,
                    Year = x.Year,
                    BrandCode = (x.Type != null && x.Type.Brand != null) ? x.Type.Brand.Code : string.Empty,
                    BrandName = (x.Type != null && x.Type.Brand != null) ? x.Type.Brand.Name : string.Empty,
                    Code = x.Code,
                    Name = x.Name
                })
                .FirstOrDefaultAsync();

            return type;
        }

        public async Task<bool> UpdateMstModel(ReqUpdateMstModelDto dto, int id)
        {
            
            var model = await _context.MstModels
                .FirstOrDefaultAsync(t => t.Id == id && t.DeletedAt == null);

            if (model == null)
            {
                throw new Exception("Model is not found.");
            }

            var targetType = await _context.MstTypes
            .Include(t => t.Brand)
            .FirstOrDefaultAsync(t => t.Id == dto.TypeId && t.DeletedAt == null);

            if (targetType == null)
            {
                throw new Exception("Type is not found.");
            }

            var isDuplicate = await _context.MstModels
                .AnyAsync(m =>
                    m.Code.ToLower() == dto.Code.ToLower() &&
                    m.Type.Code.ToLower() == targetType.Code.ToLower() &&
                    m.Type.Brand.Code.ToLower() == targetType.Brand.Code.ToLower() &&
                    m.Id != id &&
                    m.DeletedAt == null);

            if (isDuplicate)
            {
                throw new Exception($"Brand '{targetType.Brand.Code}', Type '{targetType.Code}', and Model '{dto.Code}' is already exists!");
            }

            model.TypeId = dto.TypeId;
            model.Code = dto.Code;
            model.Name = dto.Name;
            model.IsActive = dto.IsActive;
            model.Year = dto.Year;
            model.UpdatedAt = DateTime.Now;
            model.UpdatedBy = dto.UpdatedBy;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}