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
    public class MasterBrandsController : Controller
    {
        private readonly IMstBrandsService _mstBrandsService;

        public MasterBrandsController(IMstBrandsService mstBrandsService)
        {
            _mstBrandsService = mstBrandsService;
        }

        public async Task<IActionResult> Index(string search, int page = 1, int limit = 3)
        {
            var param = new ReqBaseParamDto
            {
                Search = search,
                Page = page,
                Limit = limit
            };
            var result = await _mstBrandsService.GetMstBrands(param);
            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReqCreateMstBrandDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            await _mstBrandsService.CreateMstBrand(dto);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            var result = await _mstBrandsService.GetMstBrandsById(id);
            if (result == null)
            {
                return NotFound();
            }

            var dto = new ReqUpdateMstBrandDto
            {
                Code = result.Code,
                Name = result.Name
            };

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] int id, ReqUpdateMstBrandDto dto)
        {
            dto.UpdatedBy = "SYSTEM";
            ModelState.Remove(nameof(dto.UpdatedBy));

            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var updated = await _mstBrandsService.UpdateMstBrand(id, dto);
            if (!updated)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}