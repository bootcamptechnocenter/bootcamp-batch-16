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
    // [Authorize]
    public class MasterGeneralController : Controller
    {
        private readonly IMstBrandService _service;
        private readonly IMstTypeService _serviceType;
        public MasterGeneralController(IMstBrandService service, IMstTypeService serviceType)
        {
            _service = service;
            _serviceType = serviceType;
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
        public async Task<ActionResult<ApiResponse<object>>> UpdateBrand(int id, [FromBody] ReqUpdateMstBrandDto dto)
        {
            await _service.UpdateMstBrand(id, dto);
            return Ok(ApiResponse<object>.Success(
                null,
                "brand updated successfully"
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

        // TYPE ENDPOINTS
        [HttpGet("type")]
        public async Task<ActionResult<ApiResponse<object>>> GetTypes([FromQuery] ReqBaseParamDto dto)
        {
            var result = await _serviceType.GetMstTypes(dto);
            return Ok(ApiResponse<object>.Success(
                result.Items,
                result.Items != null ? "success" : "data not found",
                result.Pagination
            ));
        }
        [HttpGet("type/{id}")]
        public async Task<ActionResult<ApiResponse<object>>> GetTypeById(int id)
        {
            var result = await _serviceType.GetMstTypeById(id);
            return Ok(ApiResponse<object>.Success(
                result,
                result != null ? "success" : "data not found"
            ));
        }
        [HttpPost("type")]
        public async Task<ActionResult<ApiResponse<object>>> CreateType([FromBody] ReqCreateMstType dto)
        {
            await _serviceType.CreateMstType(dto);
            return Ok(ApiResponse<object>.Success(
                null,
                "type created successfully"
            ));
        }
        [HttpPut("type/{id}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateType(int id, [FromBody] ReqUpdateMstTypeDto dto)
        {
            await _serviceType.UpdateMstType(id, dto);
            return Ok(ApiResponse<object>.Success(
                null,
                "type updated successfully"
            ));
        }
        [HttpDelete("type/{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteType(int id, [FromQuery] string deletedBy)
        {
            try
            {
                await _serviceType.DeleteMstType(id, deletedBy);

                return Ok(ApiResponse<object>.Success(
                    null,
                    "type deleted successfully"
                ));
            }
            catch (Exception ex)
            {
    
                return BadRequest(ApiResponse<object>.Success(
                    null,
                    ex.Message
                ));
            }
        }

    }
}