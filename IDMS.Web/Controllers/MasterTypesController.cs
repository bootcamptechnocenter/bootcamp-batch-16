using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Modules.Master.Dto.Request;
using IDMS.Web.Services;
using IDMS.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace IDMS.Web.Controllers
{
    public class MasterTypesController : Controller
    {
        private readonly IMstTypesService _mstTypesService;
        private readonly IMstBrandsService _mstBrandsService;

        public MasterTypesController(IMstTypesService mstTypesService, IMstBrandsService mstBrandsService)
        {
            _mstTypesService = mstTypesService;
            _mstBrandsService = mstBrandsService;
        }
        public async Task<IActionResult> Index(string search, int page = 1, int limit = 5)
        {
            var dto = new ReqBaseParamDto
            {
                Search = search,
                Page = page,
                Limit = limit
            };

            var result = await _mstTypesService.GetAllTypes(dto);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return PartialView("_IndexTable", result);
            return View(result);
        }

        private async Task PopulateBrandsDropdown()
        {
            var brands = await _mstBrandsService.GetMstBrands(new ReqBaseParamDto { Page = 1, Limit = 1000 });
            ViewBag.BrandList = new SelectList(brands.Items, "Id", "Name");
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PopulateBrandsDropdown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReqCreateTypeDto dto)
        {
            if (ModelState.IsValid)
            {
                await _mstTypesService.CreateType(dto);
                TempData["Toast"] = "Type created successfully.";
                TempData["ToastType"] = "success";
                return RedirectToAction(nameof(Index));
            }
            await PopulateBrandsDropdown();
            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            var result = await _mstTypesService.GetTypeById(id);
            if (result == null)
            {
                return NotFound();
            }

            var dto = new ReqUpdateTypeDto
            {
                BrandId = result.BrandId,
                Code = result.Code,
                Name = result.Name,
                IsActive = result.IsActive,
                UpdatedBy = "SYSTEM"
            };
            await PopulateBrandsDropdown();
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, ReqUpdateTypeDto dto)
        {
            dto.UpdatedBy = "SYSTEM";
            ModelState.Remove(nameof(dto.UpdatedBy));

            if (ModelState.IsValid)
            {
                await _mstTypesService.UpdateType(id, dto);
                TempData["Toast"] = "Type updated successfully.";
                TempData["ToastType"] = "success";
                return RedirectToAction(nameof(Index));
            }
            await PopulateBrandsDropdown();
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _mstTypesService.DeleteType(id);
            TempData["Toast"] = "Type deleted successfully.";
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