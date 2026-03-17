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
        private readonly ICurrentUserService _currentUserService;

        public MstModelService(AppDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }


        public async Task<PagedResult<ResMstModelDto>> GetMstModel(ReqBaseParamDto dto)
        {
            var query = _context.MstModels
                .Include(x => x.Type)
                    .ThenInclude(t => t.Brand)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(dto.Search))
                {
                    var search = dto.Search.ToLower();

                    query = query.Where(x =>
                        x.Code.ToLower().Contains(search) ||
                        x.Name.ToLower().Contains(search) ||
                        x.Type.Name.ToLower().Contains(search) ||
                        x.Type.Code.ToLower().Contains(search) ||
                        x.Type.Brand.Name.ToLower().Contains(search) ||
                        x.Type.Brand.Code.ToLower().Contains(search)
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
                    TypeName = x.Type != null ? x.Type.Name : string.Empty,
                    TypeCode = x.Type != null ? x.Type.Code : string.Empty,
                    BrandId = x.Type != null ? x.Type.BrandId : 0,
                    BrandName = x.Type != null && x.Type.Brand != null
                        ? x.Type.Brand.Name
                        : string.Empty,
                    Code = x.Code,
                    Name = x.Name,
                    Year = x.Year
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
                .Include(x => x.Type)
                    .ThenInclude(t => t.Brand)
                .Where(x => x.Id == id && x.DeletedAt == null)
                .Select(x => new ResMstModelDto
                {
                    Id = x.Id,
                    TypeId = x.TypeId,
                    TypeName = x.Type != null ? x.Type.Name : string.Empty,
                    TypeCode = x.Type != null ? x.Type.Code : string.Empty,
                    BrandId = x.Type != null ? x.Type.BrandId : 0,
                    BrandName = x.Type != null && x.Type.Brand != null
                        ? x.Type.Brand.Name
                        : string.Empty,

                    Code = x.Code,
                    Name = x.Name,
                    Year = x.Year
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
                CreatedAt = DateTime.Now,
                CreatedBy = dto.CreatedBy
            };

            _context.MstModels.Add(model);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateMstModel(int id, ReqUpdateMstModelDto dto)
        {
            var model = await _context.MstModels.FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);
            if (model == null) return false;
            var actor = await _currentUserService.GetCurrentUserFullNameAsync();

            model.TypeId = dto.TypeId;
            model.Code = dto.Code;
            model.Name = dto.Name;
            model.Year = dto.Year;
            model.UpdatedAt = DateTime.Now;
            var fallbackUpdatedBy = string.IsNullOrWhiteSpace(dto.UpdatedBy) ? "Admin" : dto.UpdatedBy;
            model.UpdatedBy = string.IsNullOrWhiteSpace(actor) ? fallbackUpdatedBy : actor;

            _context.MstModels.Update(model);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteMstModel(int id)
        {
            var stock = await _context.MstModels
                .FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);
            if (stock == null) return false;

            stock.DeletedAt = DateTime.Now;
            stock.DeletedBy = "system";

            await _context.SaveChangesAsync();
            return true;
        }
    }
}