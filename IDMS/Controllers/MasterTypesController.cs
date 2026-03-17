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
    public class MasterTypesController : ControllerBase
    {
        private readonly IMstTypeService _service;
        public MasterTypesController(IMstTypeService service)
        {
            _service = service;
        }

        [HttpGet("type")]
        public async Task<ActionResult<ApiResponse<object>>> GetMstType([FromQuery] ReqBaseParamDto dto)
        {
            var result = await _service.GetMstType(dto);
            return Ok(ApiResponse<object>.Success(result.Items, result.Items != null ? "success" : "data not found", result.Pagination));
        }

        [HttpGet("type/{id}")]
        public async Task<ActionResult<ApiResponse<object>>> GetMstTypeById(int id)
        {
            var result = await _service.GetMstTypeById(id);
            return Ok(ApiResponse<object>.Success(result, result != null ? "success" : "data not found"));
        }

        [HttpPost("type")]
        public async Task<ActionResult<ApiResponse<object>>> CreateMstType([FromBody] ReqCreateMstTypeDto dto)
        {
            await _service.CreateMstType(dto);
            return Ok(ApiResponse<object>.Success(null, "type created successfully"));
        }

        [HttpPut("type/{id}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateMstType(int id, [FromBody] ReqUpdateMstTypeDto dto)
        {
            var result = await _service.UpdateMstType(id, dto);
            return Ok(ApiResponse<object>.Success(
                null,
                result ? "type updated successfully" : "type not found"
            ));
        }

        [HttpDelete("type/{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteMstType(int id, [FromQuery] string deletedBy)
        {
            var result = await _service.DeleteMstType(id, deletedBy);
            return Ok(ApiResponse<object>.Success(
                null,
                result ? "type deleted successfully" : "type not found"
            ));
        }
    }
}