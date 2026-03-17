using IDMS.Modules.Master.Dto.Request;
using IDMS.Shared.Entities;
using IDMS.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            var param = new ReqBaseParamDto
            {
                Search = search,
                Page = page,
                Limit = limit
            };

            var result = await _service.GetMstModel(param);
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadTypeOptions();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReqCreateMstModelDto model)
        {
            if (!ModelState.IsValid)
            {
                await LoadTypeOptions(model.TypeId);
                return View(model);
            }

            await _service.CreateMstModel(model);
            return RedirectToAction(nameof(Index));
        }

        private async Task LoadTypeOptions(int? selectedTypeId = null)
        {
            var types = await _typeService.GetMstType(new ReqBaseParamDto
            {
                Page = 1,
                Limit = 1000,
                Search = string.Empty
            });

            ViewBag.TypeOptions = types.Items.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = $"{x.Name} ({x.Code})",
                Selected = selectedTypeId.HasValue && x.Id == selectedTypeId.Value
            }).ToList();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var mstModel = await _service.GetMstModelById(id);
            if (mstModel == null)
            {
                return NotFound();
            }

            ViewBag.ModelId = id;
            var model = new ReqUpdateMstModelDto
            {
                TypeId = mstModel.TypeId,
                Code = mstModel.Code,
                Name = mstModel.Name,
                Year = mstModel.Year,
                IsActive = mstModel.IsActive
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ReqUpdateMstModelDto model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ModelId = id;
                return View(model);
            }

            model.UpdatedBy = User.Identity?.Name ?? "system";
            var updated = await _service.UpdateMstModel(id, model);
            if (!updated)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var deletedBy = User.Identity?.Name ?? "system";
            await _service.DeleteMstModel(id, deletedBy);
            return RedirectToAction(nameof(Index));
        }
    }
}
