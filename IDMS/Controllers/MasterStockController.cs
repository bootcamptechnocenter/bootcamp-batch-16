using IDMS.Common;
using IDMS.Modules.Master.Dto.Request;
using IDMS.Modules.Master.Services;
using IDMS.Shared.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IDMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MasterStockController : ControllerBase
    {


        private readonly IMstStockService _stockService;

        public MasterStockController(
                    IMstStockService stockService)
        {
            _stockService = stockService;
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