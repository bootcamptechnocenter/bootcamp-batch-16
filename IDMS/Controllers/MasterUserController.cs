using System.Threading.Tasks;
using IDMS.Common;
using IDMS.Modules.Master.Dto.Request;
using IDMS.Modules.Master.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IDMS.Controllers
{
    [ApiController]
    [Route("master/user")]
    public class MasterUserController : ControllerBase
    {
        private readonly IMstUserService _userService;

        public MasterUserController(IMstUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<object>>> CreateMstUser([FromBody] ReqCreateMstUserDto dto)
        {
            var result = await _userService.CreateMstUser(dto);
            return Ok(ApiResponse<object>.Success(result, result != null ? "user created successfully" : "user creation failed"));
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<object>>> Login([FromBody] ReqLoginDto dto)
        {
            var result = await _userService.Login(dto);
            return Ok(ApiResponse<object>.Success(result, "login successful"));
        }
    }
}
