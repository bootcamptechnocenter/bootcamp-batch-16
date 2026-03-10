using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Common;
using IDMS.Modules.Master.Services;
using IDMS.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IDMS.Controllers
{
    [Route("[controller]")]
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
            result.Items != null ? "success" : "data not found",
            result.Pagination
           ));
        }
    }
}