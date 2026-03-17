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
    public class MstModelController : Controller

    {
        private readonly IMstModelService _service;

        public MstModelController(IMstModelService service)
        {
            _service = service;
        }

        [HttpGet("model")]
        public async Task<IActionResult> GetMstModel([FromQuery] ReqBaseParamDto dto)
        {
            var result = await _service.GetMstModel(dto);
            return Ok(ApiResponse<object>.Success(
                result.Items,
                result.Items != null ? "success" : "data not found",
                result.Pagination
            ));
        }

        [HttpGet("model/available")]
        public async Task<IActionResult> GetAvailableMstModel([FromQuery] ReqBaseParamDto dto)
        {
            var result = await _service.GetAvailableMstModel(dto);
            return Ok(ApiResponse<object>.Success(
                result.Items,
                result.Items != null ? "success" : "data not found",
                result.Pagination
            ));
        }
    }
}