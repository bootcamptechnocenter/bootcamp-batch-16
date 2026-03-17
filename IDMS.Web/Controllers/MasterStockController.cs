using System;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Modules.Master.Dto.Request;
using IDMS.Shared.Entities;
using IDMS.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace IDMS.Web.Controllers
{
    public class MasterStockController : Controller
    {
        private readonly IMasterStockService _service;
        private readonly IMasterModelService _modelService;

        public MasterStockController(IMasterStockService service, IMasterModelService modelService)
        {
            _service = service;
            _modelService = modelService;
        }

        // ================= INDEX =================
        public async Task<IActionResult> Index(string search, int page = 1, int limit = 10)
        {
            var param = BuildParam(search, page, limit);
            var result = await _service.GetMstStock(param);
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Table(string search, int page = 1, int limit = 10)
        {
            var param = BuildParam(search, page, limit);
            var result = await _service.GetMstStock(param);
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

        // ================= CREATE =================
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReqCreateMstStockDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.SelectedModelText = await GetModelText(dto.ModelId);
                return View(dto);
            }

            await _service.CreateMstStock(dto);
            return RedirectToAction(nameof(Index));
        }

        // ================= EDIT =================
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _service.GetMstStockById(id);
            if (result == null) return NotFound();

            var dto = new ReqUpdateMstStockDto
            {
                ModelId = result.ModelId,
                StockAmount = result.StockAmount,
                Price = result.Price
            };

            ViewBag.StockId = id;
            ViewBag.SelectedModelText = $"{result.ModelCode} — {result.ModelName}";

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ReqUpdateMstStockDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.StockId = id;
                ViewBag.SelectedModelText = await GetModelText(dto.ModelId);
                return View(dto);
            }

            var updated = await _service.UpdateMstStock(dto, id);
            if (!updated) return NotFound();

            return RedirectToAction(nameof(Index));
        }

        // ================= DELETE =================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteMstStock(id);
            return RedirectToAction(nameof(Index));
        }

        // ================= DROPDOWN MODEL =================
        [HttpGet]
        public async Task<IActionResult> ModelOptions(string search = "", int page = 1, int limit = 10)
        {
            var result = await _modelService.GetMstModel(new ReqBaseParamDto
            {
                Search = search,
                Page = page,
                Limit = limit
            });

            var items = result.Items.Select(model => new
            {
                id = model.Id,
                text = $"{model.Code} — {model.Name}"
            });

            return Json(new
            {
                items,
                hasMore = result.Pagination.HasNextPage
            });
        }

        // ================= HELPER =================
        private async Task<string?> GetModelText(int modelId)
        {
            if (modelId <= 0) return null;

            var model = await _modelService.GetMstModelById(modelId);
            if (model == null) return null;

            return $"{model.Code} — {model.Name}";
        }

        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}