using Microsoft.AspNetCore.Mvc;
using WebApi.Common;
using WebApi.Modules.Master.Services;
using WebApi.Shared.Entities;

namespace WebApi.Controllers
{
    public class MasterGeneralController(IMstBrandService service) : Controller
    {
        private readonly IMstBrandService _service = service;

        [HttpGet("brands")]
        public async Task<ActionResult<ApiResponse<object>>> GetBrands([FromQuery] ReqBaseParamDto dto)
        {
            var result = await _service.GetMstBrands(dto);
            return Ok(ApiResponse<object>.Success(
                result.Items,
                result.Items != null ? "Success" : "Data not found",
                result.Pagination
            ));
        }
    }
}