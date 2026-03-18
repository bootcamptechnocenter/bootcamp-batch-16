using IDMS.Common;
using IDMS.Modules.Master.Dto.Request;
using IDMS.Modules.Master.Services;
using IDMS.Shared.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IDMS.Controllers
{
    [ApiController]
    [Route("master/general/brand")]
    [Authorize]
    public class MasterGeneralBrandController : ControllerBase
    {
        private readonly IMstBrandService _service;

        public MasterGeneralBrandController(IMstBrandService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<object>>> GetMstBrand([FromQuery] ReqBaseParamDto dto)
        {
            var result = await _service.GetMstBrand(dto);
            return Ok(ApiResponse<object>.Success(result.Items, result.Items != null ? "success" : "data not found", result.Pagination));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> GetMstBrandById(int id)
        {
            var result = await _service.GetMstBrandById(id);
            if (result == null)
            {
                return NotFound(ApiResponse<object>.Fail("brand not found"));
            }

            return Ok(ApiResponse<object>.Success(result, "success"));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<object>>> CreateMstBrand([FromBody] ReqCreateMstBrancDto dto)
        {
            await _service.CreateMstBrand(dto);
            return Ok(ApiResponse<object>.Success(null, "Brand created successfully"));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateMstBrand(int id, [FromBody] ReqUpdateMstBrancDto dto)
        {
            var result = await _service.UpdateMstBrand(id, dto);
            if (!result)
            {
                return NotFound(ApiResponse<object>.Fail("brand not found"));
            }

            return Ok(ApiResponse<object>.Success(null, "brand updated successfully"));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteMstBrand(int id, [FromQuery] string deletedBy)
        {
            var result = await _service.DeleteMstBrand(id, deletedBy);
            if (!result)
            {
                return NotFound(ApiResponse<object>.Fail("brand not found"));
            }

            return Ok(ApiResponse<object>.Success(null, "brand deleted successfully"));
        }
    }
}