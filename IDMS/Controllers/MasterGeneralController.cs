using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    [Route("[controller]")]
    public class MasterGeneralController : Controller
    {
        private readonly IMstBrandService _service;
        public MasterGeneralController(IMstBrandService service)
        {
            _service = service;
        }

        [HttpGet("brand")]
        [Authorize]
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
        [Authorize]
        public async Task<ActionResult<ApiResponse<object>>> GetBrandById(int id)
        {
            var result = await _service.GetMstBrandById(id);
            return Ok(ApiResponse<object>.Success(
                result,
                result != null ? "success" : "data not found"
            ));
        }

        [HttpPost("brand")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<object>>> CreateBrand([FromBody] ReqCreateMstBrandDto dto)
        {
            await _service.CreateMstBrand(dto);
            return Ok(ApiResponse<object>.Success(null, "Brand created successfully"));
        }

        [HttpPut("brand/{id}")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<object>>> UpdateBrand(int id, [FromBody] ReqUpdateMstBrandDto dto)
        {
            await _service.UpdateMstBrand(id, dto);
            return Ok(ApiResponse<object>.Success(null, "Brand updated successfully"));
        }

        [HttpDelete("brand/{id}")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<object>>> DeleteBrand(int id)
        {
            var success = await _service.DeleteMstBrand(id);
            if(success)
            {
                return Ok(ApiResponse<object>.Success(null, "Brand deleted successfully"));
            }
            else
            {
                return NotFound(ApiResponse<object>.Fail("Brand not found"));
            }
        }
    }
}