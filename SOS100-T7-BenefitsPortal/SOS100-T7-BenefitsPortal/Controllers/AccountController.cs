using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SOS100_T7_BenefitsPortal.Services;

namespace SOS100_T7_BenefitsPortal.Controllers;

public class AccountController : Controller
{
    private readonly UserApiService _userApiService;

    public AccountController(UserApiService userApiService)
    {
        _userApiService = userApiService;
    }

    [HttpGet]
    public IActionResult Login(string returnUrl)
    {
        ViewBag.ReturnUrl = returnUrl;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password, string returnUrl)
    {
        var user = await _userApiService.LoginAsync(email, password);

        if (user == null)
        {
            ViewBag.Error = "Fel e-postadress eller lösenord.";
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
        identity.AddClaim(new Claim(ClaimTypes.Name, user.Name));
        identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
        identity.AddClaim(new Claim(ClaimTypes.Role, user.Role));
        identity.AddClaim(new Claim("UserId", user.Id.ToString()));

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity));

        if (string.IsNullOrEmpty(returnUrl))
            return RedirectToAction("Index", "Home");

        return Redirect(returnUrl);
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }
}
