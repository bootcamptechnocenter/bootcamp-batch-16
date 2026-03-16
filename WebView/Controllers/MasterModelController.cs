using Microsoft.AspNetCore.Mvc;
using WebApi.Modules.Master.Dto.Request;
using WebApi.Shared.Entities;
using WebView.Services;

namespace WebView.Controllers
{

    public class MasterModelController(IMstModelClientService modelService, IMstTypeClientService typeService) : Controller
    {
        private readonly IMstModelClientService _modelService = modelService;
        private readonly IMstTypeClientService _typeService = typeService;

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

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var model = await _modelService.GetMstModelById(id);
                var updateDto = new ReqMstModelUpdateDto
                {
                    TypeId = model.TypeId,
                    Code = model.Code,
                    Name = model.Name,
                    Year = model.Year,
                    IsActive = model.IsActive
                };
                var types = _typeService.GetMstTypes(new ReqBaseParamDto { Limit = int.MaxValue }).Result.Items;
                ViewBag.Types = types;
                ViewBag.ModelId = id;
                return View(updateDto);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Model not found: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ReqMstModelUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                var types = _typeService.GetMstTypes(new ReqBaseParamDto { Limit = int.MaxValue }).Result.Items;
                ViewBag.Types = types;
                ViewBag.ModelId = id;
                return View(dto);
            }

            try
            {
                await _modelService.UpdateMstModel(id, dto);
                TempData["SuccessMessage"] = "Model updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error updating model: {ex.Message}");
                var types = _typeService.GetMstTypes(new ReqBaseParamDto { Limit = int.MaxValue }).Result.Items;
                ViewBag.Types = types;
                ViewBag.ModelId = id;
                return View(dto);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _modelService.DeleteMstModel(id);
                TempData["SuccessMessage"] = "Model deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting model: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
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