using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Common;
using IDMS.Module.Master.Services;
using IDMS.Shared.Entities;
using Microsoft.AspNetCore.Mvc;

namespace IDMS.Controllers
{
    [ApiController]
    [Route("master/general")]
    public class MasterGeneralController : Controller
    {
        private readonly IMstBrandService _service;

        public MasterGeneralController(IMstBrandService service)
        {
            _service = service;
        }

        [HttpGet("brand")]
        public async Task<IActionResult> GetMstBrand([FromQuery] ReqBaseParamDto dto)
        {
            var result = await _service.GetMstBrand(dto);
            return Ok(ApiResponse<object>.Success(
                result.Items,
                result.Items != null ? "success" : "data not found",
                result.Pagination
            ));
        }
    }
}