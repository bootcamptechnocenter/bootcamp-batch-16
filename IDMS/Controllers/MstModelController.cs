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
    [Authorize] // Hapus sementara kalau mau tes tanpa login token
    public class MstModelController : ControllerBase
    {
        private readonly IMstModelService _service;

        public MstModelController(IMstModelService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int limit = 10)
        {
            var param = new ReqBaseParamDto { Search = search, Page = page, Limit = limit };
            var result = await _service.GetMstModel(param);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetMstModelById(id);
            if (result == null) return NotFound(new { message = "Model not found" });
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ReqCreateMstModelDto dto)
        {
            await _service.CreateMstModel(dto);
            return Ok(new { message = "Model created successfully" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ReqUpdateMstModelDto dto)
        {
            var success = await _service.UpdateMstModel(dto, id);
            if (!success) return NotFound(new { message = "Model not found" });
            return Ok(new { message = "Model updated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteMstModel(id);
            if (!success) return NotFound(new { message = "Model not found" });
            return Ok(new { message = "Model deleted successfully" });
        }
    }
}