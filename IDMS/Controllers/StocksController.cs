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
    [Route("api/stocks")]
    [Authorize]
    public class StocksController : ControllerBase
    {
        private readonly IMstStocksService _mstStocksService;

        public StocksController(IMstStocksService mstStocksService)
        {
            _mstStocksService = mstStocksService;
        }

        [HttpGet("")]
        public async Task<ActionResult<ApiResponse<object>>> GetStocks([FromQuery] ReqBaseParamDto dto)
        {
            var result = await _mstStocksService.GetAllStocks(dto);
            return Ok(ApiResponse<object>.Success(result.Items, result.Items != null ? "Success Get Stocks" : "No Stocks Found", result.Pagination));
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<object?>>> GetStockById(int id)
        {
            var result = await _mstStocksService.GetStockById(id);
            if (result != null)
            {
                return Ok(ApiResponse<object>.Success(result, "Success Get Stock by Id"));
            }
            else
            {
                return NotFound(ApiResponse<object>.Failure($"Stock with Id of {id} Not Found"));
            }
        }
        [HttpPost("stock")]
        public async Task<ActionResult<ApiResponse<bool>>> CreateStock(ReqCreateStockDto dto)
        {
            var result = await _mstStocksService.CreateStock(dto);
            if (result)
            {
                return Ok(ApiResponse<bool>.Success(result, "Success Create Stock"));
            }
            else
            {
                return BadRequest(ApiResponse<bool>.Failure("Failed to Create Stock"));
            }
        }
        [HttpPut("stock/{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateStock(int id, ReqUpdateStockDto dto)
        {
            var result = await _mstStocksService.UpdateStock(id, dto);
            if (result)
            {
                return Ok(ApiResponse<bool>.Success(result, "Success Update Stock"));
            }
            else
            {
                return BadRequest(ApiResponse<bool>.Failure("Failed to Update Stock"));
            }
        }
        [HttpDelete("stock/{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteStock(int id)
        {
            var result = await _mstStocksService.DeleteStock(id);
            if (result)
            {
                return Ok(ApiResponse<bool>.Success(result, "Success Delete Stock"));
            }
            else
            {
                return NotFound(ApiResponse<bool>.Failure($"Failed to Delete Stock with Id of {id}"));
            }
        }
    }
}