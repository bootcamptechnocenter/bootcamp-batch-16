using System.Diagnostics;
using IDMS.Modules.Master.Dto.Request;
using IDMS.Shared.Entities;
using IDMS.Web.Models;
using IDMS.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IDMS.Web.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly IMstBrandsService _brandsService;
    private readonly IMstTypesService _typesService;
    private readonly IMstModelsService _modelsService;
    private readonly IMstStocksService _stocksService;

    public HomeController(
        IMstBrandsService brandsService,
        IMstTypesService typesService,
        IMstModelsService modelsService,
        IMstStocksService stocksService)
    {
        _brandsService = brandsService;
        _typesService = typesService;
        _modelsService = modelsService;
        _stocksService = stocksService;
    }

    public async Task<IActionResult> Index()
    {
        var bigParam = new ReqBaseParamDto { Page = 1, Limit = 9999 };
        var smallParam = new ReqBaseParamDto { Page = 1, Limit = 1 };

        var brandsTask = _brandsService.GetMstBrands(smallParam);
        var typesTask = _typesService.GetAllTypes(bigParam);
        var modelsTask = _modelsService.GetAllModels(smallParam);
        var stocksTask = _stocksService.GetAllStocks(bigParam);

        await Task.WhenAll(brandsTask, typesTask, modelsTask, stocksTask);

        var types = typesTask.Result;
        var stocks = stocksTask.Result.Items.ToList();

        // Aggregates
        var totalQty = stocks.Sum(s => s.Quantity);
        var totalValue = stocks.Sum(s => (long)s.Price * s.Quantity);
        var lowStockThreshold = 5;

        // Per-brand chart
        var byBrand = stocks
            .GroupBy(s => s.BrandName)
            .OrderByDescending(g => g.Sum(s => s.Quantity))
            .ToList();

        // Top 5 by inventory value
        var topStocks = stocks
            .OrderByDescending(s => (long)s.Price * s.Quantity)
            .Take(5)
            .Select(s => new DashboardStockRow
            {
                BrandName = s.BrandName,
                TypeName = s.TypeName,
                ModelName = s.ModelName,
                Price = s.Price,
                Quantity = s.Quantity,
                TotalValue = (long)s.Price * s.Quantity
            }).ToList();

        // Low stock items
        var lowStock = stocks
            .Where(s => s.Quantity <= lowStockThreshold)
            .OrderBy(s => s.Quantity)
            .Take(5)
            .Select(s => new DashboardStockRow
            {
                BrandName = s.BrandName,
                TypeName = s.TypeName,
                ModelName = s.ModelName,
                Price = s.Price,
                Quantity = s.Quantity,
                TotalValue = (long)s.Price * s.Quantity
            }).ToList();

        var vm = new DashboardViewModel
        {
            TotalBrands = brandsTask.Result.Pagination.TotalItems,
            TotalTypes = types.Pagination.TotalItems,
            ActiveTypes = types.Items.Count(t => t.IsActive),
            InactiveTypes = types.Items.Count(t => !t.IsActive),
            TotalModels = modelsTask.Result.Pagination.TotalItems,
            TotalStockItems = stocks.Count(),
            TotalQuantity = totalQty,
            TotalInventoryValue = totalValue,
            LowStockCount = stocks.Count(s => s.Quantity <= lowStockThreshold),
            BrandLabels = byBrand.Select(g => g.Key).ToList(),
            BrandQuantities = byBrand.Select(g => g.Sum(s => s.Quantity)).ToList(),
            TopStocks = topStocks,
            LowStockItems = lowStock,
        };

        return View(vm);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
