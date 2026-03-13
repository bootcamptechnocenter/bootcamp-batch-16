using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Modules.Master.Dto.Request;
using IDMS.Modules.Master.Services;
using IDMS.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IDMS.Web.Controllers
{
    public class MasterTypeController : Controller
    {
        private readonly IMstTypeService _service;

        public MasterTypeController(IMstTypeService service)
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
            var result = await _service.GetMstTypes(param);
            return View(result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ReqCreateMstType dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            await _service.CreateMstType(dto);
            return RedirectToAction(nameof(Index));
        }
    }
}