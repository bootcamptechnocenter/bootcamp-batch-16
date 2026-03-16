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
        private readonly IMasterBrandService _service;
        public MasterBrandController(IMasterBrandService service)
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
        public async Task<IActionResult> Create(ReqCreateMstBrancDto model)
        {
            if (ModelState.IsValid)
            {
                await _service.CreateMstBrand(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var brand = await _service.GetMstBrandById(id);
            if (brand == null)
            {
                return NotFound();
            }

            ViewBag.BrandId = id;
            var model = new ReqUpdateMstBrancDto
            {
                Code = brand.Code,
                Name = brand.Name,
                IsActive = brand.IsActive
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ReqUpdateMstBrancDto model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.BrandId = id;
                return View(model);
            }

            var updated = await _service.UpdateMstBrand(id, model);
            if (!updated)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}