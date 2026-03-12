using Microsoft.AspNetCore.Mvc;
using WebApi.Modules.Master.Dto.Request;
using WebApi.Modules.Master.Services;
using WebApi.Shared.Entities;

namespace WebView.Controllers
{

    public class MasterTypeController(IMstTypeService typeService, IMstBrandService brandService) : Controller
    {
        private readonly IMstTypeService _typeService = typeService;
        private readonly IMstBrandService _brandService = brandService;

        public async Task<IActionResult> Index(string search, int page = 1, int limit = 10)
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
    }
}