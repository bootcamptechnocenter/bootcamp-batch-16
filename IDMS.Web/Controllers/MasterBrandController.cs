using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Module.Master.Dto.Request;
using IDMS.Module.Master.Dto.Response;
using IDMS.Web.Services;
using IDMS.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace IDMS.Web.Controllers
{
    [Authorize]
    public class MasterBrandController : Controller
    {

        private readonly IMstBrandService _service;

        public MasterBrandController(IMstBrandService service)
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
            var result = await _service.GetMstBrand(param);
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

            await _service.CreateMstBrand(dto);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _service.GetMstBrandById(id);
            if (result == null)
            {
                return NotFound();
            }

            var dto = new ReqUpdateNameMstBrandDto
            {
                Name = result.Name,
                // Code = result.Code,
                // IsActive = result.IsActive  
            };
            ViewBag.BrandId = id;
            return View(dto);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ReqUpdateNameMstBrandDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.BrandId = id;
                return View(dto);
            }

            var founded = await _service.GetMstBrandById(id);
            if (founded == null)
            {
                return NotFound();
            }

            Console.WriteLine($"Updating brand with ID: {id}, New Name: {dto.Name}");

            await _service.UpdateNameMstBrand(id, dto);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Login()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }


    }
}