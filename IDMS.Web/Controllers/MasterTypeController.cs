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
        private readonly IMstBrandService _brandService;


        public MasterTypeController(IMstTypeService service, IMstBrandService brandService)
        {
            _service = service;
            _brandService = brandService;
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

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReqCreateMstTypeDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
                
            }
            await _service.CreateMstType(dto);   
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var type = await _service.GetMstTypeById(id);
            if (type == null)
            {
                return NotFound();
            }

            var brands = await _brandService.GetMstBrand(new ReqBaseParamDto { Search = "", Page = 1, Limit = 100 });
            ViewBag.BrandList = brands.Items.Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name,
                Selected = x.Id == type.BrandId
            }).ToList();

            var editDto = new ReqUpdateMstTypeDto
            {
                Id = type.Id,
                BrandId = type.BrandId,
                Code = type.Code,
                Name = type.Name,
                IsActive = type.IsActive
            };
            return View(editDto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ReqUpdateMstTypeDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
                
            }
            await _service.UpdateMstType(dto.Id, dto);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteMstType(id);
            if (success)
            {
                return View("Index");
            }
            else
            {
                return NotFound(new { message = "Type not found" });
            }
        }
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}