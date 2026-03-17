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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace IDMS.Web.Controllers
{
    [Authorize]
    public class MasterModelsController : Controller
    {
        private readonly IMstModelsService _mstModelsService;
        private readonly IMstTypesService _mstTypesService;
        private readonly IMstBrandsService _mstBrandsService;

        public MasterModelsController(IMstModelsService mstModelsService, IMstTypesService mstTypesService, IMstBrandsService mstBrandsService)
        {
            _mstModelsService = mstModelsService;
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

            var result = await _mstModelsService.GetAllModels(dto);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return PartialView("_IndexTable", result);
            return View(result);
        }

        private async Task PopulateDropdowns()
        {
            var brands = await _mstBrandsService.GetMstBrands(new ReqBaseParamDto { Page = 1, Limit = 1000 });
            ViewBag.BrandList = new SelectList(brands.Items, "Id", "Name");
        }

        [HttpGet]
        public async Task<IActionResult> GetTypesByBrand(int brandId)
        {
            var types = await _mstTypesService.GetAllTypes(new ReqBaseParamDto { Page = 1, Limit = 1000 });
            var filtered = types.Items
                .Where(t => t.BrandId == brandId)
                .Select(t => new { id = t.Id, name = t.Name });
            return Json(filtered);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PopulateDropdowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReqCreateModelDto dto)
        {
            if (ModelState.IsValid)
            {
                await _mstModelsService.CreateModel(dto);
                TempData["Toast"] = "Model created successfully.";
                TempData["ToastType"] = "success";
                return RedirectToAction(nameof(Index));
            }
            await PopulateDropdowns();
            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            var result = await _mstModelsService.GetModelById(id);
            if (result == null)
            {
                return NotFound();
            }

            var typeInfo = await _mstTypesService.GetTypeById(result.TypeId);
            ViewBag.CurrentBrandId = typeInfo?.BrandId ?? 0;

            var dto = new ReqUpdateModelDto
            {
                TypeId = result.TypeId,
                Name = result.Name,
                Code = result.Code,
                Year = result.Year,
                UpdatedBy = "SYSTEM"
            };
            await PopulateDropdowns();
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, ReqUpdateModelDto dto)
        {
            dto.UpdatedBy = "SYSTEM";
            ModelState.Remove(nameof(dto.UpdatedBy));

            if (ModelState.IsValid)
            {
                await _mstModelsService.UpdateModel(id, dto);
                TempData["Toast"] = "Model updated successfully.";
                TempData["ToastType"] = "success";
                return RedirectToAction(nameof(Index));
            }
            await PopulateDropdowns();
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _mstModelsService.DeleteModel(id);
            TempData["Toast"] = "Model deleted successfully.";
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