using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using IDMS.Common;
using IDMS.Modules.Master.Dto.Request;
using IDMS.Modules.Master.Services;
using Microsoft.AspNetCore.Mvc;

namespace IDMS.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class UserController : ControllerBase
    {
        private readonly IMstUsersService _mstUsersService;

        public UserController(IMstUsersService mstUsersService)
        {
            _mstUsersService = mstUsersService;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] ReqLoginUser dto)
        {
            try
            {
                var result = await _mstUsersService.LoginUser(dto);
                return Ok(ApiResponse<object>.Success(result, "Login successful", null));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ApiResponse<object>.Failure(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.Failure(ex.Message));
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] ReqRegisterUser dto)
        {
            try
            {
                var result = await _mstUsersService.RegisterUser(dto);
                return Ok(ApiResponse<object>.Success(result, "Registration successful", null));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<object>.Failure(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.Failure(ex.Message));
            }
        }
    }
}