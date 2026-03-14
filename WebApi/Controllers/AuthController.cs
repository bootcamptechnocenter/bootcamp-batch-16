using Microsoft.AspNetCore.Mvc;
using WebApi.Common;
using WebApi.Modules.Master.Dto.Request;
using WebApi.Modules.Master.Services;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController(IAuthService authService) : Controller
    {
        private readonly IAuthService _authService = authService;

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<object>>> Register([FromBody] ReqAuthRegisterDto dto)
        {
            var result = await _authService.Register(dto);
            return Ok(ApiResponse<object>.Success(
                result,
                "User registered successfully"
            ));
        }
        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<object>>> Login([FromBody] ReqAuthLoginDto dto)
        {
            var result = await _authService.Login(dto);
            return Ok(ApiResponse<object>.Success(
                result,
                "User logged in successfully"
            ));
        }
    }
}