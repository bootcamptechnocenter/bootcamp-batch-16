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
    public class MasterTypeController : Controller
    {
        private readonly IMasterTypeService _service;
        private readonly IMasterBrandService _brandService;

        public MasterTypeController(IMasterTypeService service, IMasterBrandService brandService)
        {
            _service = service;
            _brandService = brandService;
        }

        public async Task<IActionResult> Index(string search, int page = 1, int limit = 10)
        {
            var param = BuildParam(search, page, limit);
            var result = await _service.GetMstType(param);
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Table(string search, int page = 1, int limit = 10)
        {
            var param = BuildParam(search, page, limit);
            var result = await _service.GetMstType(param);
            return PartialView("_MasterTypeTable", result);
        }

        private static ReqGetTypeDto BuildParam(string search, int page, int limit)
        {
            return new ReqGetTypeDto
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
        public async Task<IActionResult> Create(ReqCreateMstTypeDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.SelectedBrandText = await GetBrandText(dto.BrandId);
                return View(dto);
            }

            await _service.CreateMstType(dto);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _service.GetMstTypeById(id);
            if (result == null) return NotFound();

            var dto = new ReqUpdateMstTypeDto
            {
                BrandId = result.BrandId,
                Code = result.Code,
                Name = result.Name,
                IsActive = true
            };

            ViewBag.TypeId = id;
            ViewBag.SelectedBrandText = $"{result.BrandCode} — {result.BrandName}";
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ReqUpdateMstTypeDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.TypeId = id;
                ViewBag.SelectedBrandText = await GetBrandText(dto.BrandId);
                return View(dto);
            }

            var updated = await _service.UpdateMstType(dto, id);
            if (!updated) return NotFound();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteMstType(id);
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
