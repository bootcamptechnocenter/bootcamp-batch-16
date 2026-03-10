using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Common;
using IDMS.Modules.Master.Services;
using IDMS.Shared.Entities;
using Microsoft.AspNetCore.Mvc;

namespace IDMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
    }
}