using Microsoft.AspNetCore.Mvc;
using WebApi.Modules.Master.Services;
using WebApi.Shared.Entities;

namespace WebView.Controllers
{

    public class MasterBrandController(IMstBrandService service) : Controller
    {
        private readonly IMstBrandService _service = service;

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
        public async Task<IActionResult> Create(WebApi.Modules.Master.Dto.Request.ReqMstBrandDto dto)
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}