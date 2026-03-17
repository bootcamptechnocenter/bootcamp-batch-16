using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Modules.Master.Dto.Request;
using IDMS.Modules.Master.Services;
using IDMS.Shared.Entities;
using IDMS.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IDMS.Web.Controllers
{
    public class MasterBrandController : Controller
    {
        private readonly IMasterBrandClientService _service;

        public MasterBrandController(IMasterBrandClientService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index(string search, int page = 1, int limit = 10)
        {
            var param = new ReqBaseParamDto
            {
                Search = search,
                Page = page,
                Limit = limit
            };

            var result = await _service.GetMstBrand(param);
            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReqCreateMstBrandDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
                
            }
            await _service.CreateMstBrand(dto);   
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var brand = await _service.GetMstBrandById(id);
            if (brand == null)
            {
                return NotFound();
            }
            var editDto = new ReqUpdateMstBrandDto
            {
                Id = brand.Id,
                Code = brand.Code,
                Name = brand.Name,
                IsActive = brand.IsActive
            };
            return View(editDto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ReqUpdateMstBrandDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
                
            }
            await _service.UpdateMstBrand(dto, id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteMstBrand(id);
            if (success)
            {
                return View("index");
            }
            else
            {
                return NotFound(new { message = "Brand not found" });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}