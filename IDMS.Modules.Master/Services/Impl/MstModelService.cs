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
            var model = new MstModel
            {
                TypeId = dto.TypeId,
                Code = dto.Code,
                Name = dto.Name,
                IsActive = true,
                CreatedAt = DateTime.Now,
                CreatedBy = dto.CreatedBy
            };

            _context.MstModels.Add(model);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteMstModel(int id)
        {
            var model = await _context.MstModels.FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);
            if (model == null)
            {
                return false;
            }

            model.DeletedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<PagedResult<ResMstModelDto>> GetMstModel(ReqGetMstModelDto dto)
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
                    (x.Type != null && x.Type.Code.ToLower().Contains(search)) ||
                    (x.Type != null && x.Type.Name.ToLower().Contains(search)) ||
                    (x.Type != null && x.Type.Brand != null && x.Type.Brand.Code.ToLower().Contains(search)) ||
                    (x.Type != null && x.Type.Brand != null && x.Type.Brand.Name.ToLower().Contains(search))
                );
            }

            if (dto.TypeId.HasValue)
            {
                query = query.Where(x => x.TypeId == dto.TypeId.Value);
            }

            query = query.Where(x => x.DeletedAt == null);

            var totalCount = await query.CountAsync();

            var page = dto.Page > 0 ? dto.Page : 1;
            var pageSize = dto.Limit > 0 ? dto.Limit : 10;
            var skip = (page - 1) * pageSize;
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var items = await query
                .OrderBy(x => x.Type.BrandId)
                .ThenBy(x => x.TypeId)
                .ThenBy(x => x.Id)
                .Skip(skip)
                .Take(pageSize)
                .Select(x => new ResMstModelDto
                {
                    Id = x.Id,
                    TypeId = x.TypeId,
                    TypeName = x.Type != null ? x.Type.Name : string.Empty,
                    BrandId = x.Type != null ? x.Type.BrandId : 0,
                    BrandName = x.Type != null && x.Type.Brand != null ? x.Type.Brand.Name : string.Empty,
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
            var model = await _context.MstModels
                 .Include(x => x.Type)
                 .ThenInclude(x => x.Brand)
                 .Where(x => x.Id == id && x.DeletedAt == null)
                 .Select(x => new ResMstModelDto
                 {
                     Id = x.Id,
                     TypeId = x.TypeId,
                     TypeName = x.Type != null ? x.Type.Name : string.Empty,
                     BrandId = x.Type != null ? x.Type.BrandId : 0,
                     BrandName = x.Type != null && x.Type.Brand != null ? x.Type.Brand.Name : string.Empty,
                     Code = x.Code,
                     Name = x.Name
                 })
                 .FirstOrDefaultAsync();

            return model;
        }

        public async Task<bool> UpdateMstModel(ReqUpdateMstModelDto dto, int id)
        {
            var model = await _context.MstModels
                .FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);

            model.TypeId = dto.TypeId;
            model.Code = dto.Code;
            model.Name = dto.Name;
            model.UpdatedAt = DateTime.Now;
            model.UpdatedBy = dto.UpdatedBy;

            await _context.SaveChangesAsync();
            return true;

        }
    }
}