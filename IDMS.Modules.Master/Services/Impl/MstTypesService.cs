using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using IDMS.Modules.Master.Dto.Request;
using IDMS.Modules.Master.Dto.Response;
using IDMS.Shared.Entities;
using IDMS.Shared.Domain.Entities;

namespace IDMS.Modules.Master.Services.Impl
{
    public class MstTypesService : IMstTypesService
    {
        private readonly AppDbContext _dbContext;

        public MstTypesService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Task<bool> CreateType(ReqCreateTypeDto dto)
        {
            var newType = new MstTypes
            {
                Name = dto.Name,
                Code = dto.Code,
                BrandId = dto.BrandId
            };

            _dbContext.MstTypes.Add(newType);
            var result = _dbContext.SaveChanges();
            return Task.FromResult(result > 0);
        }

        public Task<bool> DeleteType(int id)
        {
            var type = _dbContext.MstTypes.FirstOrDefault(x => x.Id == id && x.DeletedAt == null);
            if (type == null)
            {
                return Task.FromResult(false);
            }

            type.DeletedAt = DateTime.Now;
            type.DeletedBy = "System";
            _dbContext.MstTypes.Update(type);
            var result = _dbContext.SaveChanges();
            return Task.FromResult(result > 0);
        }

        public async Task<PagedResult<ResMstTypeDto>> GetAllTypes(ReqBaseParamDto dto)
        {
            var query = _dbContext.MstTypes.Include(x => x.Brand).AsQueryable();

            if (!string.IsNullOrWhiteSpace(dto.Search))
            {
                var search = dto.Search.ToLower();
                query = query.Where(x => x.Code != null && x.Code.ToLower().Contains(search) ||
                                         x.Name != null && x.Name.ToLower().Contains(search));
            }

            query = query.Where(x => x.DeletedAt == null);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((dto.Page - 1) * dto.Limit)
                .Take(dto.Limit)
                .Select(x => new ResMstTypeDto
                {
                    Id = x.Id,
                    BrandId = x.BrandId,
                    Brand = x.Brand.Name,
                    Name = x.Name,
                    Code = x.Code,
                    IsActive = x.IsActive
                })
                .ToListAsync();

            return new PagedResult<ResMstTypeDto>
            {
                Items = items,
                Pagination = new Pagination
                {
                    TotalItems = totalCount,
                    CurrentPage = dto.Page,
                    Limit = dto.Limit,
                    TotalPages = (int)Math.Ceiling(totalCount / (double)dto.Limit)
                }
            };

        }

        public async Task<ResMstTypeDto?> GetTypeById(int id)
        {
            var type = await _dbContext.MstTypes
                .Include(x => x.Brand)
                .Where(x => x.Id == id && x.DeletedAt == null)
                .Select(x => new ResMstTypeDto
                {
                    Id = x.Id,
                    BrandId = x.BrandId,
                    Brand = x.Brand.Name,
                    Name = x.Name,
                    Code = x.Code,
                    IsActive = x.IsActive
                })
                .FirstOrDefaultAsync();
            return type;

        }

        public Task<bool> UpdateType(int id, ReqUpdateTypeDto dto)
        {
            var type = _dbContext.MstTypes.FirstOrDefault(x => x.Id == id && x.DeletedAt == null);
            var isBrandExist = _dbContext.MstBrands.Any(x => x.Id == dto.BrandId && x.DeletedAt == null);
            if (type == null || (dto.BrandId.HasValue && !isBrandExist))
            {
                throw new InvalidOperationException(type == null ? $"Type with Id of {id} Not Found" : $"Brand with Id of {dto.BrandId.Value} Not Found");
            }

            if (!string.IsNullOrEmpty(dto.Name)) type.Name = dto.Name;
            if (!string.IsNullOrEmpty(dto.Code)) type.Code = dto.Code;
            if (dto.BrandId.HasValue) type.BrandId = dto.BrandId.Value;
            type.UpdatedAt = DateTime.Now;
            type.UpdatedBy = "System";

            _dbContext.MstTypes.Update(type);
            var result = _dbContext.SaveChanges();
            return Task.FromResult(result > 0);
        }
    }
}