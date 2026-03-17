using System;
using System.Collections.Generic;
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
        private readonly IMasterTypeService  _typeService;
        private readonly IMasterBrandService _brandService;
        private readonly IMasterModelService _modelService;

        public MasterModelController(
            IMasterTypeService  typeService,
            IMasterBrandService brandService,
            IMasterModelService modelService)
        {
            _typeService  = typeService;
            _brandService = brandService;
            _modelService = modelService;
        }

        // ── Index ─────────────────────────────────────────────────────────────

        public async Task<IActionResult> Index(string search, int page = 1, int limit = 10)
        {
            var param  = BuildParam(search, page, limit);
            var result = await _modelService.GetMstModel(param);
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Table(string search, int page = 1, int limit = 10)
        {
            var param  = BuildParam(search, page, limit);
            var result = await _modelService.GetMstModel(param);
            return PartialView("_MasterModelTable", result);
        }

        private static ReqBaseParamDto BuildParam(string search, int page, int limit) =>
            new ReqBaseParamDto { Search = search, Page = page, Limit = limit };

        // ── Create ────────────────────────────────────────────────────────────

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReqCreateMstModelDto dto)
        {
            if (!ModelState.IsValid)
            {
                // Kembalikan teks brand jika TypeId sudah terisi tapi validasi gagal
                if (dto.TypeId > 0)
                {
                    var type = await _typeService.GetMstTypeById(dto.TypeId);
                    if (type != null)
                    {
                        ViewBag.SelectedBrandId   = type.BrandId;
                        ViewBag.SelectedBrandText = await GetBrandText(type.BrandId);
                        ViewBag.SelectedTypeId    = dto.TypeId;
                        ViewBag.SelectedTypeText  = $"{type.Code} — {type.Name}";
                    }
                }
                return View(dto);
            }

            await _modelService.CreateMstModel(dto);
            return RedirectToAction(nameof(Index));
        }

        // ── Edit ──────────────────────────────────────────────────────────────

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _modelService.GetMstModelById(id);
            if (result == null) return NotFound();

            // Ambil detail type untuk mendapatkan BrandId
            var type = await _typeService.GetMstTypeById(result.TypeId);

            var dto = new ReqUpdateMstModelDto
            {
                TypeId   = result.TypeId,
                Code     = result.Code,
                Name     = result.Name,
                Year     = result.Year,
                IsActive = result.IsActive,
            };

            ViewBag.ModelId           = id;
            ViewBag.SelectedTypeId    = result.TypeId;
            ViewBag.SelectedTypeText  = $"{result.TypeCode} — {result.TypeName}";
            ViewBag.SelectedBrandId   = type?.BrandId ?? 0;
            ViewBag.SelectedBrandText = type != null ? await GetBrandText(type.BrandId) : null;

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ReqUpdateMstModelDto dto)
        {
            if (!ModelState.IsValid)
            {
                if (dto.TypeId > 0)
                {
                    var type = await _typeService.GetMstTypeById(dto.TypeId);
                    if (type != null)
                    {
                        ViewBag.SelectedBrandId   = type.BrandId;
                        ViewBag.SelectedBrandText = await GetBrandText(type.BrandId);
                        ViewBag.SelectedTypeId    = dto.TypeId;
                        ViewBag.SelectedTypeText  = $"{type.Code} — {type.Name}";
                    }
                }
                ViewBag.ModelId = id;
                return View(dto);
            }

            var updated = await _modelService.UpdateMstModel(dto, id);
            if (!updated) return NotFound();

            return RedirectToAction(nameof(Index));
        }

        // ── Delete ────────────────────────────────────────────────────────────

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _modelService.DeleteMstModel(id);
            return RedirectToAction(nameof(Index));
        }

        // ── AJAX Options ──────────────────────────────────────────────────────

        /// <summary>
        /// Endpoint Select2 untuk daftar Brand (lazy-load dengan pagination).
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> BrandOptions(string search = "", int page = 1, int limit = 10)
        {
            var result = await _brandService.GetMstBrand(new ReqBaseParamDto
            {
                Search = search,
                Page   = page,
                Limit  = limit,
            });

            var items = result.Items.Select(brand => new
            {
                id   = brand.Id,
                text = $"{brand.Code} — {brand.Name}",
            });

            return Json(new { items, hasMore = result.Pagination.HasNextPage });
        }

        /// <summary>
        /// Endpoint Select2 untuk daftar Type, difilter berdasarkan BrandId.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> TypeOptions(string search = "", int page = 1, int limit = 10, int brandId = 0)
        {
            var result = await _typeService.GetMstType(new ReqBaseParamDto
            {
                Search = search,
                Page   = page,
                Limit  = limit,
                // Aktifkan baris berikut jika ReqBaseParamDto dan service sudah mendukung BrandId:
                // BrandId = brandId,
            });

            // Filter manual jika service belum mendukung BrandId
            var filtered = brandId > 0
                ? result.Items.Where(t => t.BrandId == brandId)
                : result.Items;

            var items = filtered.Select(type => new
            {
                id   = type.Id,
                text = $"{type.Code} — {type.Name}",
            });

            // Catatan: hasMore mungkin tidak akurat saat filter manual.
            // Solusi ideal: tambahkan BrandId ke ReqBaseParamDto dan filter di DB layer.
            return Json(new { items, hasMore = result.Pagination.HasNextPage });
        }

        // ── Private Helpers ───────────────────────────────────────────────────

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