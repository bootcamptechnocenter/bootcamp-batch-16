using System.Linq;
using System.Threading.Tasks;
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
        private readonly IMasterStockService _service;
        private readonly IMasterModelService _modelService;
        private readonly IMasterTypeService _typeService;
        private readonly IMasterBrandService _brandService;

        public MasterStockController(IMasterStockService service, IMasterModelService modelService, IMasterTypeService typeService, IMasterBrandService brandService)
        {
            _service = service;
            _modelService = modelService;
            _typeService = typeService;
            _brandService = brandService;
        }

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
                await SetSelectionsFromModelOrForm(dto.ModelId);
                return View(dto);
            }

            await _service.CreateMstStock(dto);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _service.GetMstStockById(id);
            if (result == null) return NotFound();

            var dto = new ReqUpdateMstStockDto
            {
                ModelId = result.ModelId,
                JumlahStock = result.JumlahStock,
                Harga = result.Harga
            };

            ViewBag.StockId = id;
            ViewBag.SelectedBrandId = result.BrandId;
            ViewBag.SelectedTypeId = result.TypeId;
            ViewBag.SelectedBrandText = await GetBrandText(result.BrandId) ?? result.BrandName;
            ViewBag.SelectedTypeText = await GetTypeText(result.TypeId) ?? result.TypeName;
            ViewBag.SelectedModelText = await GetModelText(result.ModelId) ?? result.ModelName;
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ReqUpdateMstStockDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.StockId = id;
                await SetSelectionsFromModelOrForm(dto.ModelId);
                return View(dto);
            }

            var updated = await _service.UpdateMstStock(dto, id);
            if (!updated) return NotFound();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteMstStock(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> BrandOptions(string search = "", int page = 1, int limit = 10)
        {
            var result = await _brandService.GetMstBrand(new ReqBaseParamDto
            {
                Search = search,
                Page = page,
                Limit = limit
            });

            var items = result.Items.Select(brand => new
            {
                id = brand.Id,
                text = $"{brand.Code} — {brand.Name}"
            });

            return Json(new
            {
                items,
                hasMore = result.Pagination.HasNextPage
            });
        }

        [HttpGet]
        public async Task<IActionResult> TypeOptions(string search = "", int? brandId = null, int page = 1, int limit = 10)
        {
            var result = await _typeService.GetMstType(new ReqGetTypeDto
            {
                Search = search,
                BrandId = brandId,
                Page = page,
                Limit = limit
            });

            var items = result.Items.Select(type => new
            {
                id = type.Id,
                text = $"{type.BrandCode} — {type.Name}"
            });

            return Json(new
            {
                items,
                hasMore = result.Pagination.HasNextPage
            });
        }

        [HttpGet]
        public async Task<IActionResult> ModelOptions(string search = "", int? typeId = null, int page = 1, int limit = 10)
        {
            var result = await _modelService.GetMstModel(new ReqGetMstModelDto
            {
                Search = search,
                TypeId = typeId,
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

        private async Task SetSelectionsFromModelOrForm(int modelId)
        {
            var model = modelId > 0 ? await _modelService.GetMstModelById(modelId) : null;
            var brandId = model?.BrandId ?? ReadBrandIdFromForm();
            var typeId = model?.TypeId ?? ReadTypeIdFromForm();

            ViewBag.SelectedBrandId = brandId;
            ViewBag.SelectedTypeId = typeId;
            ViewBag.SelectedBrandText = brandId.HasValue ? await GetBrandText(brandId.Value) : null;
            ViewBag.SelectedTypeText = typeId.HasValue ? await GetTypeText(typeId.Value) : null;
            ViewBag.SelectedModelText = model != null ? $"{model.Code} — {model.Name}" : null;
        }

        private int? ReadBrandIdFromForm()
        {
            if (int.TryParse(Request.Form["BrandId"], out var brandId) && brandId > 0)
            {
                return brandId;
            }

            return null;
        }

        private int? ReadTypeIdFromForm()
        {
            if (int.TryParse(Request.Form["TypeId"], out var typeId) && typeId > 0)
            {
                return typeId;
            }

            return null;
        }

        private async Task<string?> GetBrandText(int brandId)
        {
            if (brandId <= 0)
            {
                return null;
            }

            var brand = await _brandService.GetMstBrandById(brandId);
            if (brand == null)
            {
                return null;
            }

            return $"{brand.Code} — {brand.Name}";
        }

        private async Task<string?> GetTypeText(int typeId)
        {
            if (typeId <= 0)
            {
                return null;
            }

            var type = await _typeService.GetMstTypeById(typeId);
            if (type == null)
            {
                return null;
            }

            return $"{type.BrandCode} — {type.Name}";
        }

        private async Task<string?> GetModelText(int modelId)
        {
            if (modelId <= 0)
            {
                return null;
            }

            var model = await _modelService.GetMstModelById(modelId);
            if (model == null)
            {
                return null;
            }

            return $"{model.Code} — {model.Name}";
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }

    }
}