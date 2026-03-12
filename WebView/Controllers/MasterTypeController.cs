using Microsoft.AspNetCore.Mvc;
using WebApi.Modules.Master.Services;
using WebApi.Shared.Entities;

namespace WebView.Controllers
{

    public class MasterTypeController(IMstTypeService service) : Controller
    {
        private readonly IMstTypeService _service = service;

        public async Task<IActionResult> Index(string search, int page = 1, int limit = 10)
        {
            var param = new ReqBaseParamDto
            {
                Search = search,
                Page = page,
                Limit = limit
            };

            var Types = await _service.GetMstTypes(param);
            return View(Types);
        }

        public IActionResult Create()
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