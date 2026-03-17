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
    public class MasterGeneralController : ControllerBase
    {
        private readonly IMstBrandService _service;
        private readonly IMstTypeService _typeService;
        private readonly IMstModelService _modelService;
        private readonly IMstStockService _stockService;
        public MasterGeneralController(
            IMstBrandService service,
            IMstTypeService typeService,
            IMstModelService modelService,
            IMstStockService stockService)
        {
            _service = service;
            _typeService = typeService;
            _modelService = modelService;
            _stockService = stockService;
        }

        [HttpGet("brand")]
        public async Task<ActionResult<ApiResponse<object>>> GetMstBrand([FromQuery] ReqBaseParamDto dto)
        {
            var result = await _service.GetMstBrand(dto);
            return Ok(ApiResponse<object>.Success(result.Items, result.Items != null ? "success" : "data not found", result.Pagination));
        }

        [HttpGet("brand/{id}")]
        public async Task<ActionResult<ApiResponse<object>>> GetMstBrandById(int id)
        {
            var result = await _service.GetMstBrandById(id);
            return Ok(ApiResponse<object>.Success(result, result != null ? "success" : "data not found"));
        }

        [HttpPost("brand")]
        public async Task<ActionResult<ApiResponse<object>>> CreateMstBrand([FromBody] ReqCreateMstBrancDto dto)
        {
            await _service.CreateMstBrand(dto);
            return Ok(ApiResponse<object>.Success(null, "Brand created successfully"));
        }

        [HttpPut("brand/{id}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateMstBrand(int id, [FromBody] ReqUpdateMstBrancDto dto)
        {
            var result = await _service.UpdateMstBrand(id, dto);
            return Ok(ApiResponse<object>.Success(
                null,
                result ? "brand updated successfully" : "brand not found"
            ));
        }

        [HttpDelete("brand/{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteMstBrand(int id, [FromQuery] string deletedBy)
        {
            var result = await _service.DeleteMstBrand(id, deletedBy);
            return Ok(ApiResponse<object>.Success(
                null,
                result ? "brand deleted successfully" : "brand not found"
            ));
        }

        [HttpGet("type")]
        public async Task<ActionResult<ApiResponse<object>>> GetMstType([FromQuery] ReqBaseParamDto dto)
        {
            var result = await _typeService.GetMstType(dto);
            return Ok(ApiResponse<object>.Success(result.Items, result.Items != null ? "success" : "data not found", result.Pagination));
        }

        [HttpGet("type/{id}")]
        public async Task<ActionResult<ApiResponse<object>>> GetMstTypeById(int id)
        {
            var result = await _typeService.GetMstTypeById(id);
            return Ok(ApiResponse<object>.Success(result, result != null ? "success" : "data not found"));
        }

        [HttpPost("type")]
        public async Task<ActionResult<ApiResponse<object>>> CreateMstType([FromBody] ReqCreateMstTypeDto dto)
        {
            await _typeService.CreateMstType(dto);
            return Ok(ApiResponse<object>.Success(null, "type created successfully"));
        }

        [HttpPut("type/{id}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateMstType(int id, [FromBody] ReqUpdateMstTypeDto dto)
        {
            var result = await _typeService.UpdateMstType(id, dto);
            return Ok(ApiResponse<object>.Success(
                null,
                result ? "type updated successfully" : "type not found"
            ));
        }

        [HttpDelete("type/{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteMstType(int id, [FromQuery] string deletedBy)
        {
            var result = await _typeService.DeleteMstType(id, deletedBy);
            return Ok(ApiResponse<object>.Success(
                null,
                result ? "type deleted successfully" : "type not found"
            ));
        }

        [HttpGet("model")]
        public async Task<ActionResult<ApiResponse<object>>> GetMstModel([FromQuery] ReqBaseParamDto dto)
        {
            var result = await _modelService.GetMstModel(dto);
            return Ok(ApiResponse<object>.Success(result.Items, result.Items != null ? "success" : "data not found", result.Pagination));
        }

        [HttpGet("model/{id}")]
        public async Task<ActionResult<ApiResponse<object>>> GetMstModelById(int id)
        {
            var result = await _modelService.GetMstModelById(id);
            return Ok(ApiResponse<object>.Success(result, result != null ? "success" : "data not found"));
        }

        [HttpPost("model")]
        public async Task<ActionResult<ApiResponse<object>>> CreateMstModel([FromBody] ReqCreateMstModelDto dto)
        {
            await _modelService.CreateMstModel(dto);
            return Ok(ApiResponse<object>.Success(null, "model created successfully"));
        }

        [HttpPut("model/{id}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateMstModel(int id, [FromBody] ReqUpdateMstModelDto dto)
        {
            var result = await _modelService.UpdateMstModel(id, dto);
            return Ok(ApiResponse<object>.Success(
                null,
                result ? "model updated successfully" : "model not found"
            ));
        }

        [HttpDelete("model/{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteMstModel(int id, [FromQuery] string deletedBy)
        {
            var result = await _modelService.DeleteMstModel(id, deletedBy);
            return Ok(ApiResponse<object>.Success(
                null,
                result ? "model deleted successfully" : "model not found"
            ));
        }

        [HttpGet("stock")]
        public async Task<ActionResult<ApiResponse<object>>> GetMstStock([FromQuery] ReqBaseParamDto dto)
        {
            var result = await _stockService.GetMstStock(dto);
            return Ok(ApiResponse<object>.Success(result.Items, result.Items != null ? "success" : "data not found", result.Pagination));
        }

        [HttpGet("stock/{id}")]
        public async Task<ActionResult<ApiResponse<object>>> GetMstStockById(int id)
        {
            var result = await _stockService.GetMstStockById(id);
            return Ok(ApiResponse<object>.Success(result, result != null ? "success" : "data not found"));
        }

        [HttpPost("stock")]
        public async Task<ActionResult<ApiResponse<object>>> CreateMstStock([FromBody] ReqCreateMstStockDto dto)
        {
            await _stockService.CreateMstStock(dto);
            return Ok(ApiResponse<object>.Success(null, "stock created successfully"));
        }

        [HttpPut("stock/{id}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateMstStock(int id, [FromBody] ReqUpdateMstStockDto dto)
        {
            var result = await _stockService.UpdateMstStock(id, dto);
            return Ok(ApiResponse<object>.Success(
                null,
                result ? "stock updated successfully" : "stock not found"
            ));
        }

        [HttpDelete("stock/{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteMstStock(int id, [FromQuery] string deletedBy)
        {
            var result = await _stockService.DeleteMstStock(id, deletedBy);
            return Ok(ApiResponse<object>.Success(
                null,
                result ? "stock deleted successfully" : "stock not found"
            ));
        }

    }
}