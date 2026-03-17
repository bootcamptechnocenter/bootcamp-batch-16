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
    public class MasterModelController : Controller
    {
        private readonly IMasterModelService _service;
        private readonly IMasterTypeService _typeService;
        private readonly IMasterBrandService _brandService;

        public MasterModelController(IMasterModelService service, IMasterTypeService typeService, IMasterBrandService brandService)
        {
            _service = service;
            _typeService = typeService;
            _brandService = brandService;
        }

        public async Task<IActionResult> Index(string search, int? typeId, int page = 1, int limit = 10)
        {
            var param = BuildParam(search, typeId, page, limit);
            var result = await _service.GetMstModel(param);
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Table(string search, int? typeId, int page = 1, int limit = 10)
        {
            var param = BuildParam(search, typeId, page, limit);
            var result = await _service.GetMstModel(param);
            return PartialView("_MasterModelTable", result);
        }

        private static ReqGetMstModelDto BuildParam(string search, int? typeId, int page, int limit)
        {
            return new ReqGetMstModelDto
            {
                Search = search,
                TypeId = typeId,
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
        public async Task<IActionResult> Create(ReqCreateMstModelDto dto)
        {
            if (!ModelState.IsValid)
            {
                await SetBrandSelectionFromTypeOrForm(dto.TypeId);
                ViewBag.SelectedTypeText = await GetTypeText(dto.TypeId);
                return View(dto);
            }

            await _service.CreateMstModel(dto);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _service.GetMstModelById(id);
            if (result == null) return NotFound();

            var dto = new ReqUpdateMstModelDto
            {
                TypeId = result.TypeId,
                Code = result.Code,
                Name = result.Name,
                IsActive = true
            };

            ViewBag.ModelId = id;
            ViewBag.SelectedBrandId = result.BrandId;
            ViewBag.SelectedBrandText = await GetBrandText(result.BrandId) ?? result.BrandName;
            ViewBag.SelectedTypeText = await GetTypeText(result.TypeId);
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ReqUpdateMstModelDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ModelId = id;
                await SetBrandSelectionFromTypeOrForm(dto.TypeId);
                ViewBag.SelectedTypeText = await GetTypeText(dto.TypeId);
                return View(dto);
            }

            var updated = await _service.UpdateMstModel(dto, id);
            if (!updated) return NotFound();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteMstModel(id);
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

        private async Task SetBrandSelectionFromTypeOrForm(int typeId)
        {
            var brandId = await ResolveBrandIdFromType(typeId) ?? ReadBrandIdFromForm();

            ViewBag.SelectedBrandId = brandId;
            ViewBag.SelectedBrandText = brandId.HasValue
                ? await GetBrandText(brandId.Value)
                : null;
        }

        private async Task<int?> ResolveBrandIdFromType(int typeId)
        {
            if (typeId <= 0)
            {
                return null;
            }

            var type = await _typeService.GetMstTypeById(typeId);
            if (type == null || type.BrandId <= 0)
            {
                return null;
            }

            return type.BrandId;
        }

        private int? ReadBrandIdFromForm()
        {
            if (int.TryParse(Request.Form["BrandId"], out var brandId) && brandId > 0)
            {
                return brandId;
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}