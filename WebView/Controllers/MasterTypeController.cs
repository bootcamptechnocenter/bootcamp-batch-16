using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Modules.Master.Dto.Request;
using WebApi.Shared.Entities;
using WebView.Services;

namespace WebView.Controllers
{

    [Authorize]
    public class MasterTypeController(IMstTypeClientService typeService, IMstBrandClientService brandService) : Controller
    {
        private readonly IMstTypeClientService _typeService = typeService;
        private readonly IMstBrandClientService _brandService = brandService;

        public async Task<IActionResult> Index(string search, int page = 1, int limit = 5)
        {
            var param = new ReqBaseParamDto
            {
                Search = search,
                Page = page,
                Limit = limit
            };

            var Types = await _typeService.GetMstTypes(param);
            return View(Types);
        }

        [HttpGet]
        public async Task<IActionResult> Table(string search, int page = 1, int limit = 5)
        {
            var param = new ReqBaseParamDto
            {
                Search = search,
                Page = page,
                Limit = limit
            };

            var types = await _typeService.GetMstTypes(param);
            return PartialView("_MasterTypeTable", types);
        }

        public IActionResult Create()
        {
            try
            {
                var brands = _brandService.GetMstBrands(new ReqBaseParamDto { Limit = int.MaxValue }).Result.Items;
                ViewBag.Brands = brands;
            }
            catch (Exception ex)
            {
                ViewBag.Brands = new List<ReqMstBrandDto>();
                ModelState.AddModelError("", $"Error fetching brands: {ex.Message}");
            }
            return View();
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var type = await _typeService.GetMstTypeById(id);
                var updateDto = new ReqMstTypeUpdateDto
                {
                    BrandId = type.BrandId,
                    Code = type.Code,
                    Name = type.Name,
                    IsActive = type.IsActive
                };
                var brands = _brandService.GetMstBrands(new ReqBaseParamDto { Limit = int.MaxValue }).Result.Items;
                ViewBag.Brands = brands;
                ViewBag.TypeId = id;
                return View(updateDto);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Type not found: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ReqMstTypeUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                var brands = _brandService.GetMstBrands(new ReqBaseParamDto { Limit = int.MaxValue }).Result.Items;
                ViewBag.Brands = brands;
                ViewBag.TypeId = id;
                return View(dto);
            }

            try
            {
                await _typeService.UpdateMstType(id, dto);
                TempData["SuccessMessage"] = "Type updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error updating type: {ex.Message}");
                var brands = _brandService.GetMstBrands(new ReqBaseParamDto { Limit = int.MaxValue }).Result.Items;
                ViewBag.Brands = brands;
                ViewBag.TypeId = id;
                return View(dto);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _typeService.DeleteMstType(id);
                TempData["SuccessMessage"] = "Type deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting type: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReqMstTypeDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            try
            {
                await _typeService.CreateMstType(dto);
                TempData["SuccessMessage"] = "Type created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error creating type: {ex.Message}");
                return View(dto);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleActive(int id)
        {
            try
            {
                var type = await _typeService.GetMstTypeById(id);
                var updateDto = new ReqMstTypeUpdateDto
                {
                    IsActive = !type.IsActive
                };
                await _typeService.UpdateMstType(id, updateDto);
                return Json(new { success = true, isActive = updateDto.IsActive });
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
