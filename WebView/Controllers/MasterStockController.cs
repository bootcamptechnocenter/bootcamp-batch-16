using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApi.Modules.Master.Dto.Request;
using WebApi.Shared.Entities;
using WebView.Services;

namespace WebView.Controllers
{
    [Authorize]
    public class MasterStockController(
        IMstStockClientService stockService,
        IMstModelClientService modelService,
        IMstTypeClientService typeService,
        IMstBrandClientService brandService
    ) : Controller
    {
        private readonly IMstStockClientService _stockService = stockService;
        private readonly IMstModelClientService _modelService = modelService;
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

            var stocks = await _stockService.GetMstStocks(param);
            return View(stocks);
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

            var stocks = await _stockService.GetMstStocks(param);
            return PartialView("_MasterStockTable", stocks);
        }

        public async Task<IActionResult> Create()
        {
            try
            {
                var modelOptions = await BuildModelOptions();
                ViewBag.Models = modelOptions;
            }
            catch (Exception ex)
            {
                ViewBag.Models = new List<SelectListItem>();
                ModelState.AddModelError("", $"Error fetching models: {ex.Message}");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReqMstStockDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Models = await BuildModelOptions();
                return View(dto);
            }

            try
            {
                await _stockService.CreateMstStock(dto);
                TempData["SuccessMessage"] = "Stock created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error creating stock: {ex.Message}");
                ViewBag.Models = await BuildModelOptions();
                return View(dto);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var stock = await _stockService.GetMstStockById(id);
                var updateDto = new ReqMstStockUpdateDto
                {
                    ModelId = stock.ModelId,
                    Stock = stock.Stock,
                    Price = stock.Price
                };
                ViewBag.StockId = id;
                ViewBag.Models = await BuildModelOptions();
                return View(updateDto);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Stock not found: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ReqMstStockUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.StockId = id;
                ViewBag.Models = await BuildModelOptions();
                return View(dto);
            }

            try
            {
                await _stockService.UpdateMstStock(id, dto);
                TempData["SuccessMessage"] = "Stock updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error updating stock: {ex.Message}");
                ViewBag.StockId = id;
                ViewBag.Models = await BuildModelOptions();
                return View(dto);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _stockService.DeleteMstStock(id);
                TempData["SuccessMessage"] = "Stock deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting stock: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<List<SelectListItem>> BuildModelOptions()
        {
            var models = (await _modelService.GetMstModels(new ReqBaseParamDto { Limit = int.MaxValue })).Items;
            var types = (await _typeService.GetMstTypes(new ReqBaseParamDto { Limit = int.MaxValue })).Items;
            var brands = (await _brandService.GetMstBrands(new ReqBaseParamDto { Limit = int.MaxValue })).Items;

            var typeLookup = types.ToDictionary(t => t.Id, t => t);
            var brandLookup = brands.ToDictionary(b => b.Id, b => b);

            var options = models.Select(model =>
            {
                var typeName = typeLookup.TryGetValue(model.TypeId, out var type) ? type.Name : "";
                var brandName = typeLookup.TryGetValue(model.TypeId, out var t) && brandLookup.TryGetValue(t.BrandId, out var brand)
                    ? brand.Name
                    : "";

                var name = string.Join(" ", new[] { brandName, typeName, model.Name }.Where(x => !string.IsNullOrWhiteSpace(x)));

                return new SelectListItem
                {
                    Value = model.Id.ToString(),
                    Text = name
                };
            })
            .OrderBy(x => x.Text)
            .ToList();

            return options;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
