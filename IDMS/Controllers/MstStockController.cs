using System.Threading.Tasks;
using IDMS.Modules.Master.Dto.Request;
using IDMS.Modules.Master.Services;
using IDMS.Shared.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IDMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MstStockController : ControllerBase
    {
        private readonly IMstStockService _service;

        public MstStockController(IMstStockService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int limit = 10)
        {
            var param = new ReqBaseParamDto { Search = search, Page = page, Limit = limit };
            var result = await _service.GetMstStock(param);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetMstStockById(id);
            if (result == null) return NotFound(new { message = "Stock not found" });
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ReqCreateMstStockDto dto)
        {
            await _service.CreateMstStock(dto);
            return Ok(new { message = "Stock created successfully" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ReqUpdateMstStockDto dto)
        {
            var success = await _service.UpdateMstStock(dto, id);
            if (!success) return NotFound(new { message = "Stock not found" });
            return Ok(new { message = "Stock updated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteMstStock(id);
            if (!success) return NotFound(new { message = "Stock not found" });
            return Ok(new { message = "Stock deleted successfully" });
        }
    }
}