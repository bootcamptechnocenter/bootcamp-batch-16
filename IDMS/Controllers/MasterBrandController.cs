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
    public class MasterBrandController : ControllerBase
    {
        private readonly IMstBrandService _service;

        public MasterBrandController(IMstBrandService service
        )
        {
            _service = service;

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



    }
}