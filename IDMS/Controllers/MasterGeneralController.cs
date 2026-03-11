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
        public MasterGeneralController(IMstBrandService service)
        {
            _service = service;
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
    }
}