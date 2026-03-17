using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Common;
using IDMS.Modules.Master.Dto.Request;
using IDMS.Modules.Master.Services;
using IDMS.Shared.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IDMS.Controllers
{
    [ApiController]
    [Route("api/types")]
    [Authorize]
    public class TypeController : ControllerBase
    {
        private readonly IMstTypesService _mstTypesService;

        public TypeController(IMstTypesService mstTypesService)
        {
            _mstTypesService = mstTypesService;
        }

        [HttpGet("types")]
        public async Task<ActionResult<ApiResponse<object>>> GetTypes([FromQuery] ReqBaseParamDto dto)
        {
            var result = await _mstTypesService.GetAllTypes(dto);
            return Ok(ApiResponse<object>.Success(result.Items, result.Items != null ? "Success Get Types" : "No Types Found", result.Pagination));
        }
        [HttpGet("type/{id}")]
        public async Task<ActionResult<ApiResponse<object?>>> GetTypeById(int id)
        {
            var result = await _mstTypesService.GetTypeById(id);
            if (result != null)
            {
                return Ok(ApiResponse<object>.Success(result, "Success Get Type by Id"));
            }
            else
            {
                return NotFound(ApiResponse<object>.Failure($"Type with Id of {id} Not Found"));
            }
        }
        [HttpPost("type")]
        public async Task<ActionResult<ApiResponse<bool>>> CreateType(ReqCreateTypeDto dto)
        {
            var result = await _mstTypesService.CreateType(dto);
            if (result)
            {
                return Ok(ApiResponse<bool>.Success(result, "Success Create Type"));
            }
            else
            {
                return BadRequest(ApiResponse<bool>.Failure("Failed to Create Type"));
            }
        }
        [HttpPut("type/{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateType(int id, ReqUpdateTypeDto dto)
        {
            var result = await _mstTypesService.UpdateType(id, dto);
            if (result)
            {
                return Ok(ApiResponse<bool>.Success(result, "Success Update Type"));
            }
            else
            {
                return BadRequest(ApiResponse<bool>.Failure("Failed to Update Type"));
            }
        }
        [HttpDelete("type/{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteType(int id)
        {
            var result = await _mstTypesService.DeleteType(id);
            if (result)
            {
                return Ok(ApiResponse<bool>.Success(result, "Success Delete Type"));
            }
            else
            {
                return NotFound(ApiResponse<bool>.Failure($"Failed to Delete Type with Id of {id}"));
            }
        }
    }
}