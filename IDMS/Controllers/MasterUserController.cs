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
    [Route("auth")]
    public class MasterUserController : Controller
    {


        private readonly IMstUserService _userService;

        public MasterUserController(IMstUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("create")]
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

        [HttpPost("login")]
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