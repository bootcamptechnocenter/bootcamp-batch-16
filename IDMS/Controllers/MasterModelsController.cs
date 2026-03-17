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

    [Route("master/models")]
    [Authorize]
    public class MasterModelsController : ControllerBase
    {

        private readonly IMstModelService _service;

        public MasterModelsController(IMstModelService service)
        {
            _service = service;
        }


        [HttpGet("model")]
        public async Task<ActionResult<ApiResponse<object>>> GetMstModel([FromQuery] ReqBaseParamDto dto)
        {
            var result = await _service.GetMstModel(dto);
            return Ok(ApiResponse<object>.Success(result.Items, result.Items != null ? "success" : "data not found", result.Pagination));
        }

        [HttpGet("model/{id}")]
        public async Task<ActionResult<ApiResponse<object>>> GetMstModelById(int id)
        {
            var result = await _service.GetMstModelById(id);
            return Ok(ApiResponse<object>.Success(result, result != null ? "success" : "data not found"));
        }

        [HttpPost("model")]
        public async Task<ActionResult<ApiResponse<object>>> CreateMstModel([FromBody] ReqCreateMstModelDto dto)
        {
            await _service.CreateMstModel(dto);
            return Ok(ApiResponse<object>.Success(null, "model created successfully"));
        }

        [HttpPut("model/{id}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateMstModel(int id, [FromBody] ReqUpdateMstModelDto dto)
        {
            var result = await _service.UpdateMstModel(id, dto);
            return Ok(ApiResponse<object>.Success(
                null,
                result ? "model updated successfully" : "model not found"
            ));
        }

        [HttpDelete("model/{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteMstModel(int id, [FromQuery] string deletedBy)
        {
            var result = await _service.DeleteMstModel(id, deletedBy);
            return Ok(ApiResponse<object>.Success(
                null,
                result ? "model deleted successfully" : "model not found"
            ));
        }

    }
}