using Microsoft.AspNetCore.Mvc;
using WebApi.Common;
using WebApi.Modules.Master.Services;
using WebApi.Shared.Entities;

namespace WebApi.Controllers
{
    public class MasterGeneralController(
        IMstBrandService brandService,
        IMstTypeService typeService,
        IMstModelService modelService
    ) : Controller
    {
        private readonly IMstBrandService _brandService = brandService;
        private readonly IMstTypeService _typeService = typeService;
        private readonly IMstModelService _modelService = modelService;

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
        [HttpGet("brands/{id}")]
        public async Task<ActionResult<ApiResponse<object>>> GetBrandById(int id)
        {
            var result = await _brandService.GetMstBrandById(id);
            return Ok(ApiResponse<object>.Success(
                result,
                result != null ? "Success" : "Data not found"
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

        [HttpGet("models")]
        public async Task<ActionResult<ApiResponse<object>>> GetModels([FromQuery] ReqBaseParamDto dto)
        {
            var result = await _modelService.GetMstModels(dto);
            return Ok(ApiResponse<object>.Success(
                result.Items,
                result.Items != null ? "Success" : "Data not found",
                result.Pagination
            ));
        }
    }
}