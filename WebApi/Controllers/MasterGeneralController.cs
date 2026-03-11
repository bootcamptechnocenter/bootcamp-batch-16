using Microsoft.AspNetCore.Mvc;
using WebApi.Common;
using WebApi.Modules.Master.Services;
using WebApi.Shared.Entities;

namespace WebApi.Controllers
{
    public class MasterGeneralController(IMstBrandService brandService, IMstTypeService typeService) : Controller
    {
        private readonly IMstBrandService _brandService = brandService;
        private readonly IMstTypeService _typeService = typeService;

        [HttpGet("brands")]
        public async Task<ActionResult<ApiResponse<object>>> GetBrands([FromQuery] ReqBaseParamDto dto)
        {
            var result = await _brandService.GetMstBrands(dto);
            return Ok(ApiResponse<object>.Success(
                result.Items,
                result.Items != null ? "Success" : "Data not found",
                result.Pagination
            ));
        }

        [HttpGet("types")]
        public async Task<ActionResult<ApiResponse<object>>> GetTypes([FromQuery] ReqBaseParamDto dto)
        {
            var result = await _typeService.GetMstTypes(dto);
            return Ok(ApiResponse<object>.Success(
                result.Items,
                result.Items != null ? "Success" : "Data not found",
                result.Pagination
            ));
        }
    }
}