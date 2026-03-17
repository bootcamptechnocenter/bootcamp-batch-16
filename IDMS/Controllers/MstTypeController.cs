using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Common;
using IDMS.Module.Master.Services;
using IDMS.Shared.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IDMS.Controllers
{
    [ApiController]
    [Route("master")]
    [Authorize]
    public class MstTypeController : Controller
    {
        private readonly IMstTypeService _service;

        public MstTypeController(IMstTypeService service)
        {
            _service = service;
        }

        [HttpGet("type")]
        public async Task<IActionResult> GetMstType([FromQuery] ReqBaseParamDto dto)
        {
            var result = await _service.GetMstType(dto);
            return Ok(ApiResponse<object>.Success(
                result.Items,
                result.Items != null ? "success" : "data not found",
                result.Pagination
            ));
        }
    }
}