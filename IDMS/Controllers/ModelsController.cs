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
    [Route("api/models")]
    [Authorize]
    public class ModelsController : ControllerBase
    {
        private readonly IMstModelsService _mstModelsService;

        public ModelsController(IMstModelsService mstModelsService)
        {
            _mstModelsService = mstModelsService;
        }

        [HttpGet("models")]
        public async Task<ActionResult<ApiResponse<object>>> GetAllModels([FromQuery] ReqBaseParamDto dto)
        {
            var result = await _mstModelsService.GetAllModels(dto);
            return Ok(ApiResponse<object>.Success(result.Items, result.Items != null ? "Success Get Models" : "No Models Found", result.Pagination));
        }

        [HttpGet("model/{id}")]
        public async Task<ActionResult<ApiResponse<object?>>> GetModelById(int id)
        {
            var result = await _mstModelsService.GetModelById(id);
            if (result != null)
            {
                return Ok(ApiResponse<object>.Success(result, "Success Get Model by Id"));
            }
            else
            {
                return NotFound(ApiResponse<object>.Failure($"Model with Id of {id} Not Found"));
            }
        }

        [HttpPost("model")]
        public async Task<ActionResult<ApiResponse<bool>>> CreateModel(ReqCreateModelDto dto)
        {
            var result = await _mstModelsService.CreateModel(dto);
            if (result)
            {
                return Ok(ApiResponse<bool>.Success(result, "Success Create Model"));
            }
            else
            {
                return BadRequest(ApiResponse<bool>.Failure("Failed to Create Model"));
            }
        }

        [HttpPut("model/{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateModel(int id, ReqUpdateModelDto dto)
        {
            var result = await _mstModelsService.UpdateModel(id, dto);
            if (result)
            {
                return Ok(ApiResponse<bool>.Success(result, "Success Update Model"));
            }
            else
            {
                return BadRequest(ApiResponse<bool>.Failure("Failed to Update Model"));
            }
        }

        [HttpDelete("model/{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteModel(int id)
        {
            var result = await _mstModelsService.DeleteModel(id);
            if (result)
            {
                return Ok(ApiResponse<bool>.Success(result, "Success Delete Model"));
            }
            else
            {
                return NotFound(ApiResponse<bool>.Failure($"Failed to Delete Model with Id of {id}"));
            }
        }
    }
}
