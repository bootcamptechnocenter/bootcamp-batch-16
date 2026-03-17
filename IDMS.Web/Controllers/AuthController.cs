using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IDMS.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace IDMS.Web.Controllers
{
    [Route("auth")]
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

            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "MasterBrands");
            }

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string email, string password, string? returnUrl)
        {
            var (Success, Token, ExpiresAt, Message) = await _authClientService.LoginAsync(email, password);

            if (!Success)
            {
                ViewBag.Error = Message ?? "Login failed. Please check your credentials.";
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }

            HttpContext.Session.SetString("Token", Token);

            var claim = new List<Claim>
            {
                new Claim(ClaimTypes.Name, email),
                new Claim(ClaimTypes.Role, "User"),
                new Claim("AccessToken", Token),
            };

            var identity = new ClaimsIdentity(claim, CookieAuthenticationDefaults.AuthenticationScheme);
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

            return RedirectToAction("Index", "MasterBrands");
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