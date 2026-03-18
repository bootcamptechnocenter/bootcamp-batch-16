using IDMS.Common;
using IDMS.Modules.Master.Dto.Request;
using IDMS.Modules.Master.Services;
using IDMS.Shared.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IDMS.Controllers
{
    [ApiController]
    [Route("master/general/type")]
    [Authorize]
    public class MasterGeneralTypeController : ControllerBase
    {
        private readonly IMstTypeService _service;

        public MasterGeneralTypeController(IMstTypeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<object>>> GetMstType([FromQuery] ReqBaseParamDto dto)
        {
            var result = await _service.GetMstType(dto);
            return Ok(ApiResponse<object>.Success(result.Items, result.Items != null ? "success" : "data not found", result.Pagination));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> GetMstTypeById(int id)
        {
            var result = await _service.GetMstTypeById(id);
            if (result == null)
            {
                return NotFound(ApiResponse<object>.Fail("type not found"));
            }

            return Ok(ApiResponse<object>.Success(result, "success"));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<object>>> CreateMstType([FromBody] ReqCreateMstTypeDto dto)
        {
            await _service.CreateMstType(dto);
            return Ok(ApiResponse<object>.Success(null, "type created successfully"));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateMstType(int id, [FromBody] ReqUpdateMstTypeDto dto)
        {
            var result = await _service.UpdateMstType(id, dto);
            if (!result)
            {
                return NotFound(ApiResponse<object>.Fail("type not found"));
            }

            return Ok(ApiResponse<object>.Success(null, "type updated successfully"));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteMstType(int id, [FromQuery] string deletedBy)
        {
            var result = await _service.DeleteMstType(id, deletedBy);
            if (!result)
            {
                return NotFound(ApiResponse<object>.Fail("type not found"));
            }

            return Ok(ApiResponse<object>.Success(null, "type deleted successfully"));
        }
    }
}