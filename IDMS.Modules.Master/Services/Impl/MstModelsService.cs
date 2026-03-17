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
    public class MstModelsService : IMstModelsService
    {
        private readonly AppDbContext _dbContext;

        public MstModelsService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CreateModel(ReqCreateModelDto dto)
        {
            var newModel = new MstModels
            {
                TypeId = dto.TypeId,
                Name = dto.Name,
                Code = dto.Code,
                Year = dto.Year
            };

            _dbContext.MstModels.Add(newModel);
            var result = await _dbContext.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteModel(int id)
        {
            var model = await _dbContext.MstModels.FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);
            if (model == null)
            {
                return false;
            }

            model.DeletedAt = DateTime.Now;
            model.DeletedBy = "System";
            _dbContext.MstModels.Update(model);
            var result = await _dbContext.SaveChangesAsync();
            return result > 0;
        }

        public async Task<PagedResult<ResMstModelsDto>> GetAllModels(ReqBaseParamDto dto)
        {
            var query = _dbContext.MstModels.Include(x => x.Type).AsQueryable();

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
                .Select(x => new ResMstModelsDto
                {
                    Id = x.Id,
                    TypeId = x.TypeId,
                    Type = x.Type != null ? x.Type.Name : string.Empty,
                    Name = x.Name,
                    Code = x.Code,
                    Year = x.Year
                })
                .ToListAsync();

            return new PagedResult<ResMstModelsDto>
            {
                Items = items,
                Pagination = new Pagination
                {
                    TotalItems = totalCount,
                    CurrentPage = dto.Page,
                    Limit = dto.Limit,
                    TotalPages = (int)Math.Ceiling((double)totalCount / dto.Limit)
                }
            };
        }

        public async Task<ResMstModelsDto?> GetModelById(int id)
        {
            var model = await _dbContext.MstModels.Include(x => x.Type).FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);
            if (model == null)
            {
                return null;
            }

            return new ResMstModelsDto
            {
                Id = model.Id,
                TypeId = model.TypeId,
                Type = model.Type != null ? model.Type.Name : string.Empty,
                Name = model.Name,
                Code = model.Code,
                Year = model.Year
            };
        }

        public async Task<bool> UpdateModel(int id, ReqUpdateModelDto dto)
        {
            var model = await _dbContext.MstModels.FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);
            var isTypeExist = await _dbContext.MstTypes.AnyAsync(x => x.Id == dto.TypeId && x.DeletedAt == null);
            if (model == null || (!isTypeExist))
            {
                throw new InvalidOperationException(model == null ? $"Type with Id of {id} Not Found" : $"Brand with Id of {dto.TypeId} Not Found");
            }

            if (!string.IsNullOrEmpty(dto.Name)) model.Name = dto.Name;
            if (!string.IsNullOrEmpty(dto.Code)) model.Code = dto.Code;
            if (dto.TypeId != 0) model.TypeId = dto.TypeId;
            if (dto.Year != 0) model.Year = dto.Year;
            model.UpdatedAt = DateTime.Now;
            model.UpdatedBy = "System";

            _dbContext.MstModels.Update(model);
            var result = await _dbContext.SaveChangesAsync();
            return result > 0;
        }
    }
}