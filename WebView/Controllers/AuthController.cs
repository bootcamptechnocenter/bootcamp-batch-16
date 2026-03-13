using Microsoft.AspNetCore.Mvc;
using WebApi.Modules.Master.Dto.Request;
using WebApi.Modules.Master.Services;

namespace WebView.Controllers
{
    public class AuthController(IAuthService service) : Controller
    {
        private readonly IAuthService _service = service;

        [HttpGet("/Register")]
        public IActionResult Register()
        {
            return View("~/Views/Auth/Register.cshtml", new ReqAuthRegisterDto());
        }

        [HttpPost("/Register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(ReqAuthRegisterDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Auth/Register.cshtml", dto);
            }

            try
            {
                await _service.Register(dto);
                TempData["SuccessMessage"] = "User created successfully!";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error creating user: {ex.Message}");
                return View("~/Views/Auth/Register.cshtml", dto);
            }
        }

        [HttpGet("/Login")]
        public IActionResult Login()
        {
            return View("~/Views/Auth/Login.cshtml");
        }

        [HttpPost("/Login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(ReqAuthLoginDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Auth/Login.cshtml", dto);
            }

            try
            {
                await _service.Login(dto);
                TempData["SuccessMessage"] = "Login successful!";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Login failed: {ex.Message}");
                return View("~/Views/Auth/Login.cshtml", dto);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
