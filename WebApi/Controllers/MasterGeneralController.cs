using Microsoft.AspNetCore.Mvc;
using WebApi.Common;
using WebApi.Modules.Master.Services;
using WebApi.Shared.Entities;

namespace WebApi.Controllers
{
    public class MasterGeneralController : Controller
    {
        private readonly IMstBrandService _service;
        public MasterGeneralController(IMstBrandService service)
        {
            _service = service;
        }
        [HttpGet("brand")]
        public async Task<ActionResult<ApiResponse<object>>> GetBrand([FromQuery] ReqBaseParamDto dto)
        {
            var result = await _service.GetMstBrand(dto);
            return Ok(ApiResponse<object>.Success(
                result.Items,
                result.Items != null ? "Success" : "Data not found",
                result.Pagination
            ));
        }
    }
}