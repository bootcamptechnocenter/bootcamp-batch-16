using IDMS.Common;
using IDMS.Modules.Master.Dto.Request;
using IDMS.Modules.Master.Services;
using IDMS.Shared.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IDMS.Controllers
{
    [ApiController]
    [Route("master/general/model")]
    [Authorize]
    public class MasterGeneralModelController : ControllerBase
    {
        private readonly IMstModelService _service;

        public MasterGeneralModelController(IMstModelService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<object>>> GetMstModel([FromQuery] ReqBaseParamDto dto)
        {
            var result = await _service.GetMstModel(dto);
            return Ok(ApiResponse<object>.Success(result.Items, result.Items != null ? "success" : "data not found", result.Pagination));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> GetMstModelById(int id)
        {
            var result = await _service.GetMstModelById(id);
            if (result == null)
            {
                return NotFound(ApiResponse<object>.Fail("model not found"));
            }

            return Ok(ApiResponse<object>.Success(result, "success"));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<object>>> CreateMstModel([FromBody] ReqCreateMstModelDto dto)
        {
            await _service.CreateMstModel(dto);
            return Ok(ApiResponse<object>.Success(null, "model created successfully"));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateMstModel(int id, [FromBody] ReqUpdateMstModelDto dto)
        {
            var result = await _service.UpdateMstModel(id, dto);
            if (!result)
            {
                return NotFound(ApiResponse<object>.Fail("model not found"));
            }

            return Ok(ApiResponse<object>.Success(null, "model updated successfully"));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteMstModel(int id, [FromQuery] string deletedBy)
        {
            var result = await _service.DeleteMstModel(id, deletedBy);
            if (!result)
            {
                return NotFound(ApiResponse<object>.Fail("model not found"));
            }

            return Ok(ApiResponse<object>.Success(null, "model deleted successfully"));
        }
    }
}