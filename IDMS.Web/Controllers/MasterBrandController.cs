using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Modules.Master.Dto.Request;
using IDMS.Modules.Master.Services;
using IDMS.Shared.Entities;
using IDMS.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IDMS.Web.Controllers
{
    [Authorize]
    public class MasterBrandController : Controller
    {
        private readonly IMasterBrandService _service;

        public MasterBrandController(IMasterBrandService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index(string search, int page = 1, int limit = 10)
        {
            var param = BuildParam(search, page, limit);
            var result = await _service.GetMstBrand(param);
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Table(string search, int page = 1, int limit = 10)
        {
            var param = BuildParam(search, page, limit);
            var result = await _service.GetMstBrand(param);
            return PartialView("_MasterBrandTable", result);
        }

        private static ReqBaseParamDto BuildParam(string search, int page, int limit)
        {
            return new ReqBaseParamDto
            {
                Search = search,
                Page = page,
                Limit = limit
            };
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

            try
            {
                await _service.CreateMstBrand(dto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(dto);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _service.GetMstBrandById(id);
            if (result == null)
            {
                return NotFound();
            }

            var dto = new ReqUpdateMstBrandDto
            {
                Code = result.Code,
                Name = result.Name,
                IsActive = true
            };

            ViewBag.BrandId = id;
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ReqUpdateMstBrandDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.BrandId = id;
                return View(dto);
            }

            try
            {
                var updated = await _service.UpdateMstBrand(dto, id);
                if (!updated) return NotFound();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Tangkap error dari API dan tampilkan di UI
                ModelState.AddModelError(string.Empty, ex.Message);
                
                ViewBag.BrandId = id;
                return View(dto);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteMstBrand(id);
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}