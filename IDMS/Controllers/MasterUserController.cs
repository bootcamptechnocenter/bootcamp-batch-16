using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Common;
using IDMS.Modules.Master.Dto.Request;
using IDMS.Modules.Master.Services;
using Microsoft.AspNetCore.Mvc;

namespace IDMS.Controllers
{
    [ApiController]
    [Route("auth")]
    public class MasterUserController : Controller
    {
        private readonly IMstUserService _service;
        public MasterUserController(IMstUserService service)
        {
            _service = service;
        }
        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<object>>> CreateBrand([FromBody] ReqCreateMstUserDto dto)
        {           
            await _service.CreateUserAsync(dto);
            return Ok(ApiResponse<object>.Success(
                null,
                "user created successfully"
            ));
        }
        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<object>>> Login([FromBody] ReqLoginDto dto)
        {
            var result = await _service.Login(dto);
            return Ok(ApiResponse<object>.Success(
                result,
                "login successful"
            ));
        }
    }
}