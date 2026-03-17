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
    public class MstModelService : IMstModelService
    {
        private AppDbContext _context;

        public MstModelService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PageResult<RestMstModelDto>> GetMstModel(ReqBaseParamDto dto)
        {
            var query = _context.MstModels.Include(x => x.Type).ThenInclude(t => t.Brand).AsQueryable();

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
                .Select(x => new RestMstModelDto
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    Type = x.Type,
                    Year = x.Year,
                    MstTypeId = x.MstTypeId
                    // MstTypeName = x.Type != null ? x.Type.Name : string.Empty
                })
                .ToListAsync();
            Console.WriteLine($"items: {items}");

            return new PageResult<RestMstModelDto>
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

        public async Task<PageResult<RestMstModelDto>> GetAvailableMstModel(ReqBaseParamDto dto)
        {
            // Get model IDs that already have stock
            var modelIdsWithStock = await _context.MstStocks
                .Select(s => s.MstModelId)
                .ToListAsync();

            var query = _context.MstModels
                .Include(x => x.Type)
                    .ThenInclude(t => t.Brand)
                .Where(x => !modelIdsWithStock.Contains(x.Id)) // Exclude models with stock
                .AsQueryable();

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
                .Select(x => new RestMstModelDto
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    Type = x.Type,
                    Year = x.Year,
                    MstTypeId = x.MstTypeId
                    // MstTypeName = x.Type != null ? x.Type.Name : string.Empty
                })
                .ToListAsync();

            return new PageResult<RestMstModelDto>
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
