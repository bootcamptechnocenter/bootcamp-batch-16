using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IDMS.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace IDMS.Web.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthClientService _authClientService;
        public AuthController(IAuthClientService authClientService)
        {
            _authClientService = authClientService;
        }

        [HttpGet("login")]
        public IActionResult Login(string? returnUrl)
        {

            if(User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost("login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password, string? returnUrl)
        {
            var (Success, Token, ExpiresAt, Message) = await _authClientService.LoginAsync(username, password);
           
            if (!Success)
            {
                ViewBag.Error = Message;
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }

            HttpContext.Session.SetString("Token", Token);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, "User")
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var authProps = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = ExpiresAt
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProps);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet("register")]
        public IActionResult Register()
        {
            if(User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost("register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(IDMS.Web.Models.ReqRegisterDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var (Success, Message) = await _authClientService.RegisterAsync(dto);

            if (!Success)
            {
                ModelState.AddModelError(string.Empty, Message);
                return View(dto);
            }

            TempData["SuccessMessage"] = "Registration successful! Please login with your new account.";
            return RedirectToAction("Login");
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("Token");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Auth");
        }
    }
}