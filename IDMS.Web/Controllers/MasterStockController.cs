using IDMS.Modules.Master.Dto.Request;
using IDMS.Shared.Entities;
using IDMS.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IDMS.Web.Controllers
{
    [Authorize]
    public class MasterStockController : Controller
    {
        private readonly IMasterStockService _service;
        private readonly IMasterModelService _modelService;

        public MasterStockController(IMasterStockService service, IMasterModelService modelService)
        {
            _service = service;
            _modelService = modelService;
        }

        public async Task<IActionResult> Index(string search, int page = 1, int limit = 10)
        {
            var param = new ReqBaseParamDto
            {
                Search = search,
                Page = page,
                Limit = limit
            };

            var result = await _service.GetMstStock(param);
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadModelOptions();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReqCreateMstStockDto model)
        {
            if (!ModelState.IsValid)
            {
                await LoadModelOptions(model.ModelId);
                return View(model);
            }

            await _service.CreateMstStock(model);
            return RedirectToAction(nameof(Index));
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
                Text = $"{x.Name} ({x.Code})",
                Selected = selectedModelId.HasValue && x.Id == selectedModelId.Value
            }).ToList();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var mstStock = await _service.GetMstStockById(id);
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
            var updated = await _service.UpdateMstStock(id, model);
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
            var deletedBy = User.Identity?.Name ?? "system";
            await _service.DeleteMstStock(id, deletedBy);
            return RedirectToAction(nameof(Index));
        }
    }
}
