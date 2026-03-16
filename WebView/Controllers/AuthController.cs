using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using WebApi.Modules.Master.Dto.Request;
using WebApi.Modules.Master.Dto.Response;
using WebView.Services;

namespace WebView.Controllers
{
    [Route("[controller]")]
    public class AuthController(IAuthClientService service) : Controller
    {
        private readonly IAuthClientService _service = service;

        [HttpGet("/Register")]
        public IActionResult Register()
        {
            return View("~/Views/Auth/Register.cshtml", new ReqAuthRegisterDto());
        }

        [HttpPost("/Register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(ReqAuthRegisterDto dto)
        {
            var result = await _service.RegisterAsync(dto);

            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Message;
                return View(dto);
            }

            TempData["SuccessMessage"] = result.Message;
            return RedirectToAction("Login");
        }

        [HttpGet("Login")]
        public IActionResult Login()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }

            return View("~/Views/Auth/Login.cshtml");

        }

        [HttpPost("Login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(ReqAuthLoginDto dto, string? returnUrl)
        {
            ResAuthClientDto result = await _service.LoginAsync(dto);

            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Message;
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }
            else
            {
                TempData["SuccessMessage"] = result.Message;
            }

            HttpContext.Session.SetString("Token", result.Token);

            var claims = new List<Claim>
            {
                new(ClaimTypes.Email, dto.Email),
                new("Role", "User")
            };

            var identity = new ClaimsIdentity(claims, "CookieAuth");
            var principal = new ClaimsPrincipal(identity);
            var authProps = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = result.ExpiresAt
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProps);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost("Logout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("Token");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
