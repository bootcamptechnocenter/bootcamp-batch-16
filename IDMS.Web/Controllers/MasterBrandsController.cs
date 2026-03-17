using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Modules.Master.Dto.Request;
using IDMS.Shared.Entities;
using IDMS.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IDMS.Web.Controllers
{
    [Authorize]
    public class MasterBrandsController : Controller
    {
        private readonly IMstBrandsService _mstBrandsService;

        public MasterBrandsController(IMstBrandsService mstBrandsService)
        {
            _mstBrandsService = mstBrandsService;
        }

        public async Task<IActionResult> Index(string search, int page = 1, int limit = 5)
        {
            var param = new ReqBaseParamDto
            {
                Search = search,
                Page = page,
                Limit = limit
            };
            var result = await _mstBrandsService.GetMstBrands(param);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return PartialView("_IndexTable", result);
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

            await _mstBrandsService.CreateMstBrand(dto);

            TempData["Toast"] = "Brand created successfully.";
            TempData["ToastType"] = "success";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            var result = await _mstBrandsService.GetMstBrandsById(id);
            if (result == null)
            {
                return NotFound();
            }

            var dto = new ReqUpdateMstBrandDto
            {
                Code = result.Code,
                Name = result.Name,
                UpdatedBy = "SYSTEM"
            };

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] int id, ReqUpdateMstBrandDto dto)
        {
            dto.UpdatedBy = "SYSTEM";
            ModelState.Remove(nameof(dto.UpdatedBy));

            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            await _mstBrandsService.UpdateMstBrand(id, dto);
            TempData["Toast"] = "Brand updated successfully.";
            TempData["ToastType"] = "success";
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}