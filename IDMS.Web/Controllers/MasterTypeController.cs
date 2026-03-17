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
            var param = new ReqBaseParamDto
            {
                Search = search,
                Page = page,
                Limit = limit
            };
            var result = await _service.GetMstType(param);
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadBrandOptions();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReqCreateMstTypeDto model)
        {
            if (ModelState.IsValid)
            {
                await _service.CreateMstType(model);
                return RedirectToAction(nameof(Index));
            }

            await LoadBrandOptions(model.BrandId);
            return View(model);
        }

        private async Task LoadBrandOptions(int? selectedBrandId = null)
        {
            var brands = await _brandService.GetMstBrand(new ReqBaseParamDto
            {
                Page = 1,
                Limit = 1000,
                Search = string.Empty
            });

            ViewBag.BrandOptions = brands.Items.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = $"{x.Name} ({x.Code})",
                Selected = selectedBrandId.HasValue && x.Id == selectedBrandId.Value
            }).ToList();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var mstType = await _service.GetMstTypeById(id);
            if (mstType == null)
            {
                return NotFound();
            }

            ViewBag.TypeId = id;
            var model = new ReqUpdateMstTypeDto
            {
                BrandId = mstType.BrandId,
                Code = mstType.Code,
                Name = mstType.Name,
                IsActive = mstType.IsActive
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ReqUpdateMstTypeDto model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.TypeId = id;
                return View(model);
            }

            model.UpdatedBy = User.Identity?.Name ?? "system";
            var updated = await _service.UpdateMstType(id, model);
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
            await _service.DeleteMstType(id, deletedBy);
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}