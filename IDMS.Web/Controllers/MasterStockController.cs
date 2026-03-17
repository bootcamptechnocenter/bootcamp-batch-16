using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Modules.Master.Dto.Request;
using IDMS.Shared.Entities;
using IDMS.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IDMS.Web.Controllers
{
    public class MasterStockController : Controller
    {
        private readonly IMasterModelService _modelService;
        private readonly IMasterTypeService _service;
        private readonly IMasterBrandService _brandService;
        private readonly IMasterStockService _stockService;

        public MasterStockController(IMasterTypeService service, IMasterBrandService brandService, 
                                     IMasterModelService modelService, IMasterStockService stockService)
        {
            _service = service;
            _brandService = brandService;
            _modelService = modelService;
            _stockService = stockService;
        }

        public async Task<IActionResult> Index(string search, int page = 1, int limit = 10)
        {
            var param = BuildParam(search, page, limit);
            var result = await _stockService.GetMstStock(param);
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Table(string search, int page = 1, int limit = 10)
        {
            var param = BuildParam(search, page, limit);
            var result = await _stockService.GetMstStock(param);
            return PartialView("_MasterStockTable", result);
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

        private async Task LoadModelOptions(int? selectedModelId = null)
        {
            var models = await _modelService.GetMstModel(new ReqBaseParamDto
            {
                Page = 1,
                Limit = 1000,
                Search = string.Empty
            });

            ViewBag.ModelOptions = models.Items.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = $"{x.Code} ({x.Name})",
                Selected = selectedModelId.HasValue && x.Id == selectedModelId.Value
            }).ToList();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadModelOptions();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReqCreateMstStockDto dto)
        {
            if (!ModelState.IsValid)
            {
                await LoadModelOptions(dto.ModelId);
                return View(dto);
            }

            await _stockService.CreateMstStock(dto);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var mstStock = await _stockService.GetMstStockById(id);
            if (mstStock == null)
            {
                return NotFound();
            }

            ViewBag.StockId = id;
            var model = new ReqUpdateMstStockDto
            {
                ModelId = mstStock.ModelId,
                JumlahStock = mstStock.JumlahStock,
                Harga = mstStock.Harga
            };
            await LoadModelOptions(mstStock.ModelId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ReqUpdateMstStockDto model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.StockId = id;
                return View(model);
            }

            model.UpdatedBy = User.Identity?.Name ?? "system";
            var updated = await _stockService.UpdateMstStock(id, model);
            if (!updated)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
           await _stockService.DeleteMstStock(id);
            return RedirectToAction(nameof(Index));
        }

        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}