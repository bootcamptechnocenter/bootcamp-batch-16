using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IDMS.Web.Services;
using IDMS.Modules.Master.Dto.Request;
using IDMS.Shared.Entities; 

namespace IDMS.Web.Controllers
{
    [Authorize]
    public class MasterStockController : Controller
    {
        private readonly IMasterStockService _service;
        // Asumsi kamu sudah buat IMasterModelService untuk Select2
        // private readonly IMasterModelService _modelService; 

        public MasterStockController(IMasterStockService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index(string search, int page = 1, int limit = 10)
        {
            var param = new ReqBaseParamDto { Search = search, Page = page, Limit = limit };
            var result = await _service.GetMstStock(param);
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Table(string search, int page = 1, int limit = 10)
        {
            var param = new ReqBaseParamDto { Search = search, Page = page, Limit = limit };
            var result = await _service.GetMstStock(param);
            return PartialView("_MasterStockTable", result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReqCreateMstStockDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            // Set default user dari session jika diperlukan
            dto.CreatedBy = "System";
            await _service.CreateMstStock(dto);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteMstStock(id);
            return RedirectToAction(nameof(Index));
        }
    }
}