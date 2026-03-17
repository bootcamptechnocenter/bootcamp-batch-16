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
    [Route("api/master-general")]
    [Authorize]
    public class MasterGeneralController : ControllerBase
    {
        private readonly IMstBrandsService _mstBrandsService;


        public MasterGeneralController(IMstBrandsService mstBrandsService)
        {
            _mstBrandsService = mstBrandsService;
        }

        [HttpGet("brands")]
        public async Task<ActionResult<ApiResponse<object>>> GetBrands([FromQuery] ReqBaseParamDto dto)
        {
            var result = await _mstBrandsService.GetMstBrands(dto);
            return Ok(ApiResponse<object>.Success(result.Items, result.Items != null ? "Success Get Brands" : "No Brands Found", result.Pagination));
        }
        [HttpGet("brand/{id}")]
        public async Task<ActionResult<ApiResponse<object?>>> GetBrandsById(int id)
        {
            var result = await _mstBrandsService.GetMstBrandsById(id);
            // return Ok(ApiResponse<object?>.Success(result, result != null ? "Success Get Brand by Id" : $"Brand with Id of {id} Not Found", null));
            if (result != null)
            {
                return Ok(ApiResponse<object>.Success(result, "Success Get Brand by Id"));
            }
            else
            {
                return NotFound(ApiResponse<object>.Failure($"Brand with Id of {id} Not Found"));
            }
        }
        [HttpPost("brand")]
        public async Task<ActionResult<ApiResponse<object?>>> CreateBrand([FromBody] ReqCreateMstBrandDto dto)
        {
            await _mstBrandsService.CreateMstBrand(dto);
            return Ok(ApiResponse<object>.Success(null, "Brand Added Successfully"));
        }
        [HttpPut("brand/{id}")]
        public async Task<ActionResult<ApiResponse<object?>>> UpdateBrand(int id, [FromBody] ReqUpdateMstBrandDto dto)
        {
            try
            {
                var result = await _mstBrandsService.UpdateMstBrand(id, dto);
                if (result)
                {
                    return Ok(ApiResponse<object?>.Success(null, "Brand Updated Successfully"));
                }
                else
                {
                    return NotFound(ApiResponse<object>.Failure($"Brand with Id of {id} Not Found"));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.Failure(ex.Message));
            }
        }
        [HttpDelete("brand/{id}")]
        public async Task<ActionResult<ApiResponse<object?>>> DeleteBrand(int id)
        {
            try
            {
                var result = await _mstBrandsService.DeleteMstBrand(id);
                if (result)
                {
                    return Ok(ApiResponse<object?>.Success(null, "Brand Deleted Successfully"));
                }
                else
                {
                    return NotFound(ApiResponse<object>.Failure($"Brand with Id of {id} Not Found"));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.Failure(ex.Message));
            }
        }
    }
}