using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Common;
using IDMS.Module.Master.Dto.Request;
using IDMS.Module.Master.Services;
using IDMS.Shared.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IDMS.Controllers
{
    [ApiController]
    [Route("master/general")]
    [Authorize]
    public class MasterGeneralController : Controller
    {
        private readonly IMstBrandService _service;

        private readonly IMstUserService _userService;

        public MasterGeneralController(IMstBrandService service, IMstUserService userService)
        {
            _service = service;
            _userService = userService;
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

        [HttpGet("brand/{id}")]
        public async Task<IActionResult> GetMstBrandById(int id)
        {
            var result = await _service.GetMstBrandById(id);
            return Ok(ApiResponse<object>.Success(
                result,
                result != null ? "success" : "data not found"
            ));
        }

        [HttpPost("brand")]
        public async Task<ActionResult<ApiResponse<object>>> CreateMstBrand([FromBody] ReqCreateMstBrandDto dto)
        {
            await _service.CreateMstBrand(dto);
            return Ok(ApiResponse<object>.Success(null, "Brand created successfully"));
        }

        [HttpPut("brand/{id}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateNameMstBrand(int id, [FromBody] ReqUpdateNameMstBrandDto dto)
        {
            await _service.UpdateNameMstBrand(id, dto);
            return Ok(ApiResponse<object>.Success(
                null,
                "Brand updated successfully"
            ));
        }

        [HttpPost("user")]
        public async Task<ActionResult<ApiResponse<object>>> CreateMstUser([FromBody] ReqCreateMstUserDto dto)
        {
            try
            {
                await _userService.CreateMstUser(dto);
                return Ok(ApiResponse<object>.Success(null, "User created successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.Fail(ex.Message));
            }
        }

        [HttpPost("user/login")]
        public async Task<ActionResult<ApiResponse<object>>> Login([FromBody] ReqLoginDto dto)
        {
            try
            {
                var result = await _userService.Login(dto);
                return Ok(ApiResponse<object>.Success(result, "Login successful"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.Fail(ex.Message));
            }
        }
    }

}