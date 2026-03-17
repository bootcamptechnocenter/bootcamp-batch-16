using IDMS.Modules.Master.Dto.Request;
using IDMS.Modules.Master.Dto.Response;
using IDMS.Shared.Entities;
using IDMS.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IDMS.Web.Controllers
{
    [Authorize]
    public class MasterStockController : Controller
    {
        private readonly IMasterStockService _stockService;
        private readonly IMasterModelService _modelService;
        private readonly IMasterTypeService  _typeService;
        private readonly IMasterBrandService _brandService;

        public MasterStockController(
            IMasterStockService stockService,
            IMasterModelService modelService,
            IMasterTypeService  typeService,
            IMasterBrandService brandService)
        {
            _stockService = stockService;
            _modelService = modelService;
            _typeService  = typeService;
            _brandService = brandService;
        }

        // ── Index ─────────────────────────────────────────────────────────────

        public async Task<IActionResult> Index(string search, int page = 1, int limit = 10, int? modelId = null)
        {
            var param  = BuildParam(search, page, limit, modelId);
            var result = await _stockService.GetMstStock(param);

            // kalau dari model page, tampilkan info model di ViewBag
            if (modelId.HasValue && modelId.Value > 0)
            {
                var model = await _modelService.GetMstModelById(modelId.Value);
                ViewBag.FilterModelId   = modelId;
                ViewBag.FilterModelName = model != null ? $"{model.Code} — {model.Name}" : null;
            }

            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Table(string search, int page = 1, int limit = 10, int? modelId = null)
        {
            var param  = BuildParam(search, page, limit, modelId);
            var result = await _stockService.GetMstStock(param);
            return PartialView("_MasterStockTable", result);
        }

        private static ReqGetMstStockDto BuildParam(string search, int page, int limit, int? modelId = null) =>
            new ReqGetMstStockDto { Search = search, Page = page, Limit = limit, ModelId = modelId };

        // ── Create ────────────────────────────────────────────────────────────

        [HttpGet]
        public async Task<IActionResult> Create(int? modelId = null)
        {
            var dto = new ReqCreateMstStockDto();

            if (modelId.HasValue && modelId.Value > 0)
            {
                var model = await _modelService.GetMstModelById(modelId.Value);
                if (model != null)
                {
                    dto.ModelId = model.Id;

                    ViewBag.SelectedModelId   = model.Id;
                    ViewBag.SelectedModelText = $"{model.Code} — {model.Name}";
                    ViewBag.SelectedTypeId    = model.TypeId;
                    ViewBag.SelectedTypeText  = $"{model.TypeCode} — {model.TypeName}";
                    ViewBag.SelectedBrandId   = model.BrandId;
                    ViewBag.SelectedBrandText = $"{model.BrandCode} — {model.BrandName}";
                    ViewBag.ModelLocked       = true;  // flag untuk disable di view
                }
            }

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReqCreateMstStockDto dto)
        {
            if (!ModelState.IsValid)
            {
                await PopulateModelViewBag(dto.ModelId);
                return View(dto);
            }

            await _stockService.CreateMstStock(dto);
            return RedirectToAction(nameof(Index));
        }

        // ── Edit ──────────────────────────────────────────────────────────────

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _stockService.GetMstStockById(id);
            if (result == null) return NotFound();

            var dto = new ReqUpdateMstStockDto
            {
                ModelId     = result.ModelId,
                Color       = result.Color,
                PoliceNumber = result.PoliceNumber,
                NewUsed     = result.NewUsed,
                IsReady     = result.IsReady,
                Price       = result.Price,
            };

            ViewBag.StockId            = id;
            ViewBag.SelectedModelId    = result.ModelId;
            ViewBag.SelectedModelText  = $"{result.ModelCode} — {result.ModelName}";
            ViewBag.SelectedTypeId     = result.TypeId;
            ViewBag.SelectedTypeText   = $"{result.TypeCode} — {result.TypeName}";
            ViewBag.SelectedBrandId    = result.BrandId;
            ViewBag.SelectedBrandText  = $"{result.BrandCode} — {result.BrandName}";

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ReqUpdateMstStockDto dto)
        {
            if (!ModelState.IsValid)
            {
                await PopulateModelViewBag(dto.ModelId);
                ViewBag.StockId = id;
                return View(dto);
            }

            var updated = await _stockService.UpdateMstStock(dto, id);
            if (!updated) return NotFound();

            return RedirectToAction(nameof(Index));
        }

        // ── Delete ────────────────────────────────────────────────────────────

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _stockService.DeleteMstStock(id);
            return RedirectToAction(nameof(Index));
        }

        // ── AJAX Options ──────────────────────────────────────────────────────

        [HttpGet]
        public async Task<IActionResult> BrandOptions(string search = "", int page = 1, int limit = 10)
        {
            var result = await _brandService.GetMstBrand(new ReqBaseParamDto
            {
                Search = search,
                Page   = page,
                Limit  = limit,
            });

            var items = result.Items.Select(b => new
            {
                id   = b.Id,
                text = $"{b.Code} — {b.Name}",
            });

            return Json(new { items, hasMore = result.Pagination.HasNextPage });
        }

        [HttpGet]
        public async Task<IActionResult> TypeOptions(string search = "", int page = 1, int limit = 10, int brandId = 0)
        {
            var result = await _typeService.GetMstType(new ReqBaseParamDto
            {
                Search = search,
                Page   = page,
                Limit  = limit,
            });

            var filtered = brandId > 0
                ? result.Items.Where(t => t.BrandId == brandId)
                : result.Items;

            var items = filtered.Select(t => new
            {
                id   = t.Id,
                text = $"{t.Code} — {t.Name}",
            });

            return Json(new { items, hasMore = result.Pagination.HasNextPage });
        }

        [HttpGet]
        public async Task<IActionResult> ModelOptions(string search = "", int page = 1, int limit = 10, int typeId = 0)
        {
            var result = await _modelService.GetMstModel(new ReqBaseParamDto
            {
                Search = search,
                Page   = page,
                Limit  = limit,
            });

            var filtered = typeId > 0
                ? result.Items.Where(m => m.TypeId == typeId)
                : result.Items;

            var items = filtered.Select(m => new
            {
                id   = m.Id,
                text = $"{m.Code} — {m.Name}",
            });

            return Json(new { items, hasMore = result.Pagination.HasNextPage });
        }

        // ── Private Helpers ───────────────────────────────────────────────────

        private async Task PopulateModelViewBag(int modelId)
        {
            if (modelId <= 0) return;

            var model = await _modelService.GetMstModelById(modelId);
            if (model == null) return;

            ViewBag.SelectedModelId   = modelId;
            ViewBag.SelectedModelText = $"{model.Code} — {model.Name}";
            ViewBag.SelectedTypeId    = model.TypeId;
            ViewBag.SelectedTypeText  = $"{model.TypeCode} — {model.TypeName}";

            var type = await _typeService.GetMstTypeById(model.TypeId);
            if (type != null)
            {
                ViewBag.SelectedBrandId   = type.BrandId;
                ViewBag.SelectedBrandText = await GetBrandText(type.BrandId);
            }
        }

        private async Task<string?> GetBrandText(int brandId)
        {
            if (brandId <= 0) return null;
            var brand = await _brandService.GetMstBrandById(brandId);
            return brand == null ? null : $"{brand.Code} — {brand.Name}";
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View("Error!");
    }
}