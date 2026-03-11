using Microsoft.AspNetCore.Mvc;

namespace SOS100_T7_BenefitsPortal.Controllers;

public class AccountController : Controller
{
    public IActionResult Login() => View();

    public IActionResult Logout() => RedirectToAction("Index", "Home");
}
