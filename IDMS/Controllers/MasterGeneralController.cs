using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Common;
using IDMS.Modules.Master.Dto.Request;
using IDMS.Modules.Master.Services;
using IDMS.Shared.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IDMS.Controllers
{
    [ApiController]
    [Route("master/general")]
    [Authorize]
    public class MasterGeneralController : Controller
    {
        private readonly IMstBrandService _service;
        private readonly IMstTypeService _typeService;
        private readonly IMstModelService _modelService;

        public MasterGeneralController(IMstBrandService service, IMstTypeService typeService, IMstModelService modelService)
        {
            _service = service;
            _typeService = typeService;
            _modelService = modelService;
        }

        [HttpGet("brand")]
        public async Task<ActionResult<ApiResponse<object>>> GetBrand([FromQuery] ReqBaseParamDto dto)
        {
            var result = await _service.GetMstBrand(dto);
            return Ok(ApiResponse<object>.Success(
                result.Items,
                result.Items != null ? "success" : "data not found",
                result.Pagination
            ));
        }

        [HttpGet("brand/{id}")]
        public async Task<ActionResult<ApiResponse<object>>> GetBrandById(int id)
        {
            var result = await _service.GetMstBrandById(id);
            return Ok(ApiResponse<object>.Success(
                result,
                result != null ? "success" : "data not found"
            ));
        }

        [HttpPost("brand")]
        public async Task<ActionResult<ApiResponse<object>>> CreateBrand([FromBody] ReqCreateMstBrandDto dto)
        {
            await _service.CreateMstBrand(dto);
            return Ok(ApiResponse<object>.Success(
                null,
                "brand created successfully"
            ));
        }

        [HttpPut("brand/{id}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateBrand([FromBody] ReqUpdateMstBrandDto dto, int id)
        {
            var result = await _service.UpdateMstBrand(dto, id);
            return Ok(ApiResponse<object>.Success(
                null,
                result ? "brand updated successfully" : "brand not found"
            ));
        }

        [HttpDelete("brand/{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteBrand(int id)
        {
            var result = await _service.DeleteMstBrand(id);
            return Ok(ApiResponse<object>.Success(
                null,
                result ? "brand deleted successfully" : "brand not found"
            ));
        }

        // --- Type endpoints ---

        [HttpGet("type")]
        public async Task<ActionResult<ApiResponse<object>>> GetType([FromQuery] ReqBaseParamDto dto)
        {
            var result = await _typeService.GetMstType(dto);
            return Ok(ApiResponse<object>.Success(
                result.Items,
                result.Items != null ? "success" : "data not found",
                result.Pagination
            ));
        }

        [HttpGet("type/{id}")]
        public async Task<ActionResult<ApiResponse<object>>> GetTypeById(int id)
        {
            var result = await _typeService.GetMstTypeById(id);
            return Ok(ApiResponse<object>.Success(
                result,
                result != null ? "success" : "data not found"
            ));
        }

        [HttpPost("type")]
        public async Task<ActionResult<ApiResponse<object>>> CreateType([FromBody] ReqCreateMstTypeDto dto)
        {
            await _typeService.CreateMstType(dto);
            return Ok(ApiResponse<object>.Success(null, "type created successfully"));
        }

        [HttpPut("type/{id}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateType([FromBody] ReqUpdateMstTypeDto dto, int id)
        {
            var result = await _typeService.UpdateMstType(dto, id);
            return Ok(ApiResponse<object>.Success(null, result ? "type updated successfully" : "type not found"));
        }

        [HttpDelete("type/{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteType(int id)
        {
            var result = await _typeService.DeleteMstType(id);
            return Ok(ApiResponse<object>.Success(null, result ? "type deleted successfully" : "type not found"));
        }

        // --- Model endpoints ---

        [HttpGet("model")]
        public async Task<ActionResult<ApiResponse<object>>> GetModel([FromQuery] ReqBaseParamDto dto)
        {
            var result = await _modelService.GetMstModel(dto);
            return Ok(ApiResponse<object>.Success(
                result.Items,
                result.Items != null ? "success" : "data not found",
                result.Pagination
            ));
        }

        [HttpGet("model/{id}")]
        public async Task<ActionResult<ApiResponse<object>>> GetModelById(int id)
        {
            var result = await _modelService.GetMstModelById(id);
            return Ok(ApiResponse<object>.Success(
                result,
                result != null ? "success" : "data not found"
            ));
        }

        [HttpPost("model")]
        public async Task<ActionResult<ApiResponse<object>>> CreateModel([FromBody] ReqCreateMstModelDto dto)
        {
            await _modelService.CreateMstModel(dto);
            return Ok(ApiResponse<object>.Success(null, "model created successfully"));
        }

        [HttpPut("model/{id}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateModel([FromBody] ReqUpdateMstModelDto dto, int id)
        {
            var result = await _modelService.UpdateMstModel(dto, id);
            return Ok(ApiResponse<object>.Success(null, result ? "model updated successfully" : "model not found"));
        }

        [HttpDelete("model/{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteModel(int id)
        {
            var result = await _modelService.DeleteMstModel(id);
            return Ok(ApiResponse<object>.Success(null, result ? "model deleted successfully" : "model not found"));
        }
    }
}