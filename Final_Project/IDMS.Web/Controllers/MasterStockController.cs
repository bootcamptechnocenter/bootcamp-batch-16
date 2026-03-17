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
    public class MasterStockController : Controller
    {
        private readonly IMasterStockService _stockService;
        private readonly IMasterModelService _modelService;

        public MasterStockController(IMasterModelService modelService, IMasterStockService stockService)
        {
            _modelService = modelService;
            _stockService = stockService;
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
        public async Task<IActionResult> ModelOptions(string search = "", int page = 1, int limit = 10)
        {
            var result = await _modelService.GetMstModel(new ReqGetModelDto
            {
                Search = search,
                Page = page,
                Limit = limit,
                IsDoNotHaveStock = true
            });

            var items = result.Items.Select(model => new
            {
                id = model.Id,
                text = $"{model.BrandCode} — {model.TypeCode} — {model.Code} — {model.Name}"
            });

            return Json(new
            {
                items,
                hasMore = result.Pagination.HasNextPage
            });
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

        private async Task<string?> GetModelText(int id)
        {
            if (id <= 0) return null;
            var model = await _modelService.GetMstModelById(id);
            return model != null ? $"{model.BrandCode} — {model.TypeCode} — {model.Code} — {model.Name}" : null;
        }

        
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> Create(ReqCreateMstStockDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // Pastikan GetTypeText ditangani agar tidak melempar null ke view
                    ViewBag.SelectedModelText = await GetModelText(dto.ModelId) ?? "-- Select Model --";
                    return View(dto);
                }

                // Set default user
                dto.CreatedBy = User.Identity?.Name ?? "System";
                
                await _stockService.CreateMstStock(dto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Fail Created: {ex.Message}");
                
                ViewBag.SelectedModelText = await GetModelText(dto.ModelId) ?? "-- Select Model --";
                return View(dto);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _stockService.GetMstStockById(id);
            if (result == null) return NotFound();

            var dto = new ReqUpdateMstStockDto
            {
                Price = result.Price,
                JumlahStock = result.JumlahStock
            };

            ViewBag.StockId = id;
            ViewBag.SelectedModelText = $"{result.BrandCode} — {result.TypeCode} — {result.ModelCode} — {result.ModelName}";
            return View(dto);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ReqUpdateMstStockDto dto)
        {
            if (!ModelState.IsValid)
            {
                var result = await _stockService.GetMstStockById(id);
                if (result == null) return NotFound();
                ViewBag.SelectedModelText = $"{result.BrandCode} — {result.TypeCode} — {result.ModelCode} — {result.ModelName}";
                ViewBag.StockId = id;
                return View(dto);
            }

            try
            {
                // Panggil service
                await _stockService.UpdateMstStock(dto, id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                
                // ViewBag.SelectedTypeText = await GetModelText(dto.ModelId);
                return View(dto);
            }
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