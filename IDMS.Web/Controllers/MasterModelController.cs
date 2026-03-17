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
    public class MasterModelController : Controller
    {
        private readonly IMasterModelService _service;
        private readonly IMasterTypeService _typeService;

        public MasterModelController(IMasterModelService service, IMasterTypeService typeService)
        {
            _service = service;
            _typeService = typeService;
        }

        public async Task<IActionResult> Index(string search, int page = 1, int limit = 10)
        {
            var param = BuildParam(search, page, limit);
            var result = await _service.GetMstModel(param);
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Table(string search, int page = 1, int limit = 10)
        {
            var param = BuildParam(search, page, limit);
            var result = await _service.GetMstModel(param);
            return PartialView("_MasterModelTable", result);
        }

        private static ReqBaseParamDto BuildParam(string search, int page, int limit)
        {
            return new ReqBaseParamDto { Search = search, Page = page, Limit = limit };
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
                ViewBag.SelectedTypeText = await GetTypeText(dto.TypeId);
                return View(dto);
            }

            try
            {
                await _service.CreateMstModel(dto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                
                ViewBag.SelectedTypeText = await GetTypeText(dto.TypeId);
                return View(dto);
            }
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
                Year = result.Year,
                IsActive = true
            };

            ViewBag.ModelId = id;
            ViewBag.SelectedTypeText = $"{result.BrandName} — {result.TypeName}";
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ReqUpdateMstModelDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ModelId = id;
                ViewBag.SelectedTypeText = await GetTypeText(dto.TypeId);
                return View(dto);
            }

            try
            {
                var updated = await _service.UpdateMstModel(dto, id);
                if (!updated) return NotFound();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Tangkap error dari API dan tampilkan di UI
                ModelState.AddModelError(string.Empty, ex.Message);
                
                ViewBag.ModelId = id;
                ViewBag.SelectedTypeText = await GetTypeText(dto.TypeId);
                return View(dto);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteMstModel(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> TypeOptions(string search = "", int page = 1, int limit = 10)
        {
            var result = await _typeService.GetMstType(new ReqBaseParamDto
            {
                Search = search, Page = page, Limit = limit
            });

            var items = result.Items.Select(type => new
            {
                id = type.Id,
                text = $"{type.BrandName} — {type.Name} ({type.Code})"
            });

            return Json(new { items, hasMore = result.Pagination.HasNextPage });
        }

        private async Task<string?> GetTypeText(int typeId)
        {
            if (typeId <= 0) return null;
            var type = await _typeService.GetMstTypeById(typeId);
            return type != null ? $"{type.BrandName} — {type.Name}" : null;
        }
    }
}