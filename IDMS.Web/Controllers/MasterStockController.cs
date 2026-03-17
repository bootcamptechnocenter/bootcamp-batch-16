using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Web.Services;
using IDMS.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using IDMS.Module.Master.Dto.Request;

namespace IDMS.Web.Controllers
{
    [Authorize]
    public class MasterStockController : Controller
    {
        private readonly IMstStockService _service;

        private readonly IMstModelService _modelService;

        public MasterStockController(IMstStockService service, IMstModelService modelService)
        {
            _service = service;
            _modelService = modelService;
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

            // Get available models (without stock)
            var availableModels = await _modelService.GetAvailableMstModel(new ReqBaseParamDto
            {
                Page = 1,
                Limit = 100
            });

            ViewBag.AvailableModels = availableModels.Items;

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Index(ReqUpsertMstStockDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            await _service.UpsertMstStock(dto);
            return RedirectToAction(nameof(Index));
        }
    }
}