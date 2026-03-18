using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IDMS.Web.Services;
using IDMS.Modules.Master.Dto.Request;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Shared.Entities;


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
            var param = new ReqBaseParamDto { Search = search, Page = page, Limit = limit };
            var result = await _service.GetMstModel(param);
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Table(string search, int page = 1, int limit = 10)
        {
            var param = new ReqBaseParamDto { Search = search, Page = page, Limit = limit };
            var result = await _service.GetMstModel(param);
            return PartialView("_MasterModelTable", result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReqCreateMstModelDto dto)
        {
            if (!ModelState.IsValid)
            {
                // Kalau validasi gagal, kembalikan ke View
                return View(dto);
            }

            dto.CreatedBy = "System"; // Nanti bisa diganti ambil dari User Session
            await _service.CreateMstModel(dto);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteMstModel(id);
            return RedirectToAction(nameof(Index));
        }

        // ENDPOINT UNTUK DROPDOWN TYPE (Select2 AJAX)
        [HttpGet]
        public async Task<IActionResult> TypeOptions(string search = "", int page = 1, int limit = 10)
        {
            var result = await _typeService.GetMstType(new ReqBaseParamDto { Search = search, Page = page, Limit = limit });

            var items = result.Items.Select(t => new
            {
                id = t.Id,
                text = $"[{t.Code}] {t.BrandName} — {t.Name}"
            });

            return Json(new { items, hasMore = result.Pagination.HasNextPage });
        }
    }
}