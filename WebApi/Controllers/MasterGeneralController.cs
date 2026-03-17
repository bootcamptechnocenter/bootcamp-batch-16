using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common;
using WebApi.Modules.Master.Dto.Request;
using WebApi.Modules.Master.Services;
using WebApi.Shared.Entities;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("master/general")]
    [Authorize]
    public class MasterGeneralController(
        IMstBrandService brandService,
        IMstTypeService typeService,
        IMstModelService modelService,
        IMstStockService stockService
    ) : Controller
    {
        private readonly IMstBrandService _brandService = brandService;
        private readonly IMstTypeService _typeService = typeService;
        private readonly IMstModelService _modelService = modelService;
        private readonly IMstStockService _stockService = stockService;

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
        [HttpPost("brands")]
        public async Task<ActionResult<ApiResponse<object>>> CreateBrand([FromBody] ReqMstBrandDto dto)
        {
            var result = await _brandService.CreateMstBrand(dto);
            return Ok(ApiResponse<object>.Success(
                result,
                "Brand created successfully"
            ));
        }
        [HttpPut("brands/{id}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateBrand(int id, [FromBody] ReqMstBrandUpdateDto dto)
        {
            var result = await _brandService.UpdateMstBrand(id, dto);
            return Ok(ApiResponse<object>.Success(
                result,
                "Brand updated successfully"
            ));
        }
        [HttpDelete("brands/{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteBrand(int id)
        {
            await _brandService.DeleteMstBrand(id);
            return Ok(ApiResponse<object>.Success(
                null,
                "Brand deleted successfully"
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
        [HttpGet("types/{id}")]
        public async Task<ActionResult<ApiResponse<object>>> GetTypeById(int id)
        {
            var result = await _typeService.GetMstTypeById(id);
            return Ok(ApiResponse<object>.Success(
                result,
                result != null ? "Success" : "Data not found"
            ));
        }
        [HttpPost("types")]
        public async Task<ActionResult<ApiResponse<object>>> CreateType([FromBody] ReqMstTypeDto dto)
        {
            var result = await _typeService.CreateMstType(dto);
            return Ok(ApiResponse<object>.Success(
                result,
                "Type created successfully"
            ));
        }
        [HttpPut("types/{id}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateType(int id, [FromBody] ReqMstTypeUpdateDto dto)
        {
            var result = await _typeService.UpdateMstType(id, dto);
            return Ok(ApiResponse<object>.Success(
                result,
                "Type updated successfully"
            ));
        }
        [HttpDelete("types/{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteType(int id)
        {
            await _typeService.DeleteMstType(id);
            return Ok(ApiResponse<object>.Success(
                null,
                "Type deleted successfully"
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
        [HttpGet("models/{id}")]
        public async Task<ActionResult<ApiResponse<object>>> GetModelById(int id)
        {
            var result = await _modelService.GetMstModelById(id);
            return Ok(ApiResponse<object>.Success(
                result,
                result != null ? "Success" : "Data not found"
            ));
        }
        [HttpPost("models")]
        public async Task<ActionResult<ApiResponse<object>>> CreateModel([FromBody] ReqMstModelDto dto)
        {
            var result = await _modelService.CreateMstModel(dto);
            return Ok(ApiResponse<object>.Success(
                result,
                "Model created successfully"
            ));
        }
        [HttpPut("models/{id}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateModel(int id, [FromBody] ReqMstModelUpdateDto dto)
        {
            var result = await _modelService.UpdateMstModel(id, dto);
            return Ok(ApiResponse<object>.Success(
                result,
                "Model updated successfully"
            ));
        }
        [HttpDelete("models/{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteModel(int id)
        {
            await _modelService.DeleteMstModel(id);
            return Ok(ApiResponse<object>.Success(
                null,
                "Model deleted successfully"
            ));
        }

        [HttpGet("stocks")]
        public async Task<ActionResult<ApiResponse<object>>> GetStocks([FromQuery] ReqBaseParamDto dto)
        {
            var result = await _stockService.GetMstStocks(dto);
            return Ok(ApiResponse<object>.Success(
                result.Items,
                result.Items != null ? "Success" : "Data not found",
                result.Pagination
            ));
        }

        [HttpGet("stocks/{id}")]
        public async Task<ActionResult<ApiResponse<object>>> GetStockById(int id)
        {
            var result = await _stockService.GetMstStockById(id);
            return Ok(ApiResponse<object>.Success(
                result,
                result != null ? "Success" : "Data not found"
            ));
        }

        [HttpPost("stocks")]
        public async Task<ActionResult<ApiResponse<object>>> CreateStock([FromBody] ReqMstStockDto dto)
        {
            var result = await _stockService.CreateMstStock(dto);
            return Ok(ApiResponse<object>.Success(
                result,
                "Stock created successfully"
            ));
        }

        [HttpPut("stocks/{id}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateStock(int id, [FromBody] ReqMstStockUpdateDto dto)
        {
            var result = await _stockService.UpdateMstStock(id, dto);
            return Ok(ApiResponse<object>.Success(
                result,
                "Stock updated successfully"
            ));
        }

        [HttpDelete("stocks/{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteStock(int id)
        {
            await _stockService.DeleteMstStock(id);
            return Ok(ApiResponse<object>.Success(
                null,
                "Stock deleted successfully"
            ));
        }
    }
}
