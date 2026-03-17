using IDMS.Modules.Master.Dto.Request;
using IDMS.Shared.Entities;
using IDMS.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IDMS.Web.Controllers
{
    [Authorize]
    public class MasterStockController : Controller
    {
        private readonly IMasterStockService _service;

        public MasterStockController(IMasterStockService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index(string search, int page = 1, int limit = 10)
        {
            var param = new ReqBaseParamDto
            {
                Search = search,
                Page = page,
                Limit = limit
            };

            var result = await _service.GetMstStock(param);
            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReqCreateMstStockDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _service.CreateMstStock(model);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var mstStock = await _service.GetMstStockById(id);
            if (mstStock == null)
            {
                return NotFound();
            }

            ViewBag.StockId = id;
            var model = new ReqUpdateMstStockDto
            {
                ModelId = mstStock.ModelId,
                JumlahStock = mstStock.JumlahStock,
                Harga = mstStock.Harga
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ReqUpdateMstStockDto model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.StockId = id;
                return View(model);
            }

            model.UpdatedBy = User.Identity?.Name ?? "system";
            var updated = await _service.UpdateMstStock(id, model);
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
            await _service.DeleteMstStock(id, deletedBy);
            return RedirectToAction(nameof(Index));
        }
    }
}
