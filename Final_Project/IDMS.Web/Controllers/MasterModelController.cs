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
        private readonly IMasterTypeService _typeService;
        private readonly IMasterModelService _modelService;

        public MasterModelController(IMasterModelService modelService, IMasterTypeService typeService)
        {
            _modelService = modelService;
            _typeService = typeService;
        }

        public async Task<IActionResult> Index(string search, int page = 1, int limit = 10, bool IsDoNotHaveStock = false)
        {
            var param = BuildModel(search, page, limit, IsDoNotHaveStock);
            var result = await _modelService.GetMstModel(param);
            return View(result);
        }

        private static ReqGetModelDto BuildModel(string search, int page, int limit, bool isDoNotHaveStock = false)
        {
            return new ReqGetModelDto
            {
                Search = search,
                Page = page,
                Limit = limit,
                IsDoNotHaveStock = isDoNotHaveStock // Nilai baru masuk ke sini
            };
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
        public async Task<IActionResult> TypeOptions(string search = "", int page = 1, int limit = 10)
        {
            var result = await _typeService.GetMstType(new ReqBaseParamDto
            {
                Search = search,
                Page = page,
                Limit = limit
            });

            var items = result.Items.Select(type => new
            {
                id = type.Id,
                text = $"{type.BrandCode} — {type.Code} — {type.Name}"
            });

            return Json(new
            {
                items,
                hasMore = result.Pagination.HasNextPage
            });
        }

        [HttpGet]
        public async Task<IActionResult> Table(string search, int page = 1, int limit = 10, bool IsDoNotHaveStock = false)
        {
            var param = BuildModel(search, page, limit, IsDoNotHaveStock);
            var result = await _modelService.GetMstModel(param);
            return PartialView("_MasterModelTable", result);
        }

        private async Task<string?> GetTypeText(int id)
        {
            if (id <= 0) return null;
            var type = await _typeService.GetMstTypeById(id);
            return type != null ? $"{type.BrandCode} — {type.Code} — {type.Name}" : null;
        }

        
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> Create(ReqCreateMstModelDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // Pastikan GetTypeText ditangani agar tidak melempar null ke view
                    ViewBag.SelectedTypeText = await GetTypeText(dto.TypeId) ?? "-- Select Type --";
                    return View(dto);
                }

                // Set default user
                dto.CreatedBy = User.Identity?.Name ?? "System";
                
                await _modelService.CreateMstModel(dto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Fail Created: {ex.Message}");
                
                ViewBag.SelectedTypeText = await GetTypeText(dto.TypeId) ?? "-- Select Type --";
                return View(dto);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _modelService.GetMstModelById(id);
            if (result == null) return NotFound();

            var dto = new ReqUpdateMstModelDto
            {
                TypeId = result.TypeId,
                Year = result.Year,
                Code = result.Code,
                Name = result.Name,
                IsActive = true
            };

            ViewBag.ModelId = id;
            ViewBag.SelectedTypeText = $"{result.BrandCode} — {result.TypeCode} — {result.TypeName}";
            return View(dto);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ReqUpdateMstModelDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.SelectedTypeText = await GetTypeText(dto.TypeId);
                ViewBag.ModelId = id;
                return View(dto);
            }

            try
            {
                // Panggil service
                await _modelService.UpdateMstModel(dto, id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                
                ViewBag.SelectedTypeText = await GetTypeText(dto.TypeId);
                return View(dto);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _modelService.DeleteMstModel(id);
            return RedirectToAction(nameof(Index));
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}