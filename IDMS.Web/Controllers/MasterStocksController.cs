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
    public class MasterStocksController : Controller
    {
        private readonly IMstStocksService _mstStocksService;
        private readonly IMstBrandsService _mstBrandsService;
        private readonly IMstTypesService _mstTypesService;
        private readonly IMstModelsService _mstModelsService;

        public MasterStocksController(
            IMstStocksService mstStocksService,
            IMstBrandsService mstBrandsService,
            IMstTypesService mstTypesService,
            IMstModelsService mstModelsService)
        {
            _mstStocksService = mstStocksService;
            _mstBrandsService = mstBrandsService;
            _mstTypesService = mstTypesService;
            _mstModelsService = mstModelsService;
        }

        public async Task<IActionResult> Index(string search, int page = 1, int limit = 5)
        {
            var dto = new ReqBaseParamDto
            {
                Search = search,
                Page = page,
                Limit = limit
            };

            var result = await _mstStocksService.GetAllStocks(dto);
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
        public async Task<IActionResult> GetModelsByType(int typeId)
        {
            var models = await _mstModelsService.GetAllModels(new ReqBaseParamDto { Page = 1, Limit = 1000 });
            var filtered = models.Items
                .Where(m => m.TypeId == typeId)
                .Select(m => new { id = m.Id, name = m.Name });
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
        public async Task<IActionResult> Create(ReqCreateStockDto dto)
        {
            if (ModelState.IsValid)
            {
                await _mstStocksService.CreateStock(dto);
                TempData["Toast"] = "Stock created successfully.";
                TempData["ToastType"] = "success";
                return RedirectToAction(nameof(Index));
            }
            await PopulateDropdowns();
            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            var result = await _mstStocksService.GetStockById(id);
            if (result == null)
            {
                return NotFound();
            }

            var dto = new ReqUpdateStockDto
            {
                BrandId = result.BrandId,
                TypeId = result.TypeId,
                ModelId = result.ModelId,
                Price = result.Price,
                Quantity = result.Quantity,
                UpdatedBy = "SYSTEM"
            };
            await PopulateDropdowns();
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, ReqUpdateStockDto dto)
        {
            dto.UpdatedBy = "SYSTEM";
            ModelState.Remove(nameof(dto.UpdatedBy));

            if (ModelState.IsValid)
            {
                await _mstStocksService.UpdateStock(id, dto);
                TempData["Toast"] = "Stock updated successfully.";
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
            await _mstStocksService.DeleteStock(id);
            TempData["Toast"] = "Stock deleted successfully.";
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
