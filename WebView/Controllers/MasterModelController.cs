using Microsoft.AspNetCore.Mvc;
using WebApi.Modules.Master.Dto.Request;
using WebApi.Modules.Master.Services;
using WebApi.Shared.Entities;

namespace WebView.Controllers
{

    public class MasterModelController(IMstModelService modelService, IMstTypeService typeService) : Controller
    {
        private readonly IMstModelService _modelService = modelService;
        private readonly IMstTypeService _typeService = typeService;

        public async Task<IActionResult> Index(string search, int page = 1, int limit = 10)
        {
            var param = new ReqBaseParamDto
            {
                Search = search,
                Page = page,
                Limit = limit
            };

            var Models = await _modelService.GetMstModels(param);
            return View(Models);
        }

        public IActionResult Create()
        {
            try
            {
                var types = _typeService.GetMstTypes(new ReqBaseParamDto { Limit = int.MaxValue }).Result.Items;
                ViewBag.Types = types;
            }
            catch (Exception ex)
            {
                ViewBag.Types = new List<ReqMstTypeDto>();
                ModelState.AddModelError("", $"Error fetching types: {ex.Message}");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReqMstModelDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            try
            {
                await _modelService.CreateMstModel(dto);
                TempData["SuccessMessage"] = "Model created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error creating model: {ex.Message}");
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