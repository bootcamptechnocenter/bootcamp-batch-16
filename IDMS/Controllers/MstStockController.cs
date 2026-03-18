using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Common;
using IDMS.Module.Master.Dto.Request;
using IDMS.Module.Master.Services;
using IDMS.Shared.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IDMS.Controllers
{
    [ApiController]
    [Route("master")]
    [Authorize]
    public class MstStockController : Controller
    {
        private readonly IMstStockService _service;

        public MstStockController(IMstStockService service)
        {
            _service = service;
        }

        [HttpGet("stock")]
        public async Task<IActionResult> GetMstStock([FromQuery] ReqBaseParamDto dto)
        {
            var result = await _service.GetMstStock(dto);
            return Ok(ApiResponse<object>.Success(
               result.Items,
               result.Items != null ? "success" : "data not found",
               result.Pagination
           ));
        }


        [HttpPost("stock")]
        public async Task<IActionResult> UpsertMstStock([FromBody] ReqUpsertMstStockDto dto)
        {
            await _service.UpsertMstStock(dto);
            return Ok(ApiResponse<object>.Success(
               null,
               "success"
           ));
        }

        [HttpDelete("stock/{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteStock(int id)
        {
            var result = await _service.DeleteMstStock(id);
            return Ok(ApiResponse<object>.Success(null, result ? "stock deleted successfully" : "stock not found"));
        }
    }
}