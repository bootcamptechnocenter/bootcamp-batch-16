using IDMS.Common;
using IDMS.Modules.Master.Dto.Request;
using IDMS.Modules.Master.Services;
using IDMS.Shared.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IDMS.Controllers
{
    [ApiController]
    [Route("master/general/stock")]
    [Authorize]
    public class MasterGeneralStockController : ControllerBase
    {
        private readonly IMstStockService _service;

        public MasterGeneralStockController(IMstStockService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<object>>> GetMstStock([FromQuery] ReqBaseParamDto dto)
        {
            var result = await _service.GetMstStock(dto);
            return Ok(ApiResponse<object>.Success(result.Items, result.Items != null ? "success" : "data not found", result.Pagination));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> GetMstStockById(int id)
        {
            var result = await _service.GetMstStockById(id);
            if (result == null)
            {
                return NotFound(ApiResponse<object>.Fail("stock not found"));
            }

            return Ok(ApiResponse<object>.Success(result, "success"));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<object>>> CreateMstStock([FromBody] ReqCreateMstStockDto dto)
        {
            await _service.CreateMstStock(dto);
            return Ok(ApiResponse<object>.Success(null, "stock created successfully"));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateMstStock(int id, [FromBody] ReqUpdateMstStockDto dto)
        {
            var result = await _service.UpdateMstStock(id, dto);
            if (!result)
            {
                return NotFound(ApiResponse<object>.Fail("stock not found"));
            }

            return Ok(ApiResponse<object>.Success(null, "stock updated successfully"));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteMstStock(int id, [FromQuery] string deletedBy)
        {
            var result = await _service.DeleteMstStock(id, deletedBy);
            if (!result)
            {
                return NotFound(ApiResponse<object>.Fail("stock not found"));
            }

            return Ok(ApiResponse<object>.Success(null, "stock deleted successfully"));
        }
    }
}