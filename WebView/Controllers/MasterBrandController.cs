using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Modules.Master.Dto.Request;
using WebApi.Shared.Entities;
using WebView.Services;

namespace WebView.Controllers
{

    [Authorize]
    public class MasterBrandController(IMstBrandClientService service) : Controller
    {
        private readonly IMstBrandClientService _service = service;

        public async Task<IActionResult> Index(string search, int page = 1, int limit = 10)
        {
            var param = new ReqBaseParamDto
            {
                Search = search,
                Page = page,
                Limit = limit
            };

            var brands = await _service.GetMstBrands(param);
            return View(brands);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReqMstBrandDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            try
            {
                await _service.CreateMstBrand(dto);
                TempData["SuccessMessage"] = "Brand created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error creating brand: {ex.Message}");
                return View(dto);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var brand = await _service.GetMstBrandById(id);
                var updateDto = new ReqMstBrandUpdateDto
                {
                    Code = brand.Code,
                    Name = brand.Name,
                    IsActive = brand.IsActive
                };
                ViewBag.BrandId = id;
                return View(updateDto);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Brand not found: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ReqMstBrandUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.BrandId = id;
                return View(dto);
            }

            try
            {
                await _service.UpdateMstBrand(id, dto);
                TempData["SuccessMessage"] = "Brand updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error updating brand: {ex.Message}");
                ViewBag.BrandId = id;
                return View(dto);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteMstBrand(id);
                TempData["SuccessMessage"] = "Brand deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting brand: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
