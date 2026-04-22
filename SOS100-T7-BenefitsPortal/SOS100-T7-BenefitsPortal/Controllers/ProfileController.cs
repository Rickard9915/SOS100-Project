using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SOS100_T7_BenefitsPortal.Controllers;

[Authorize]
public class ProfileController : Controller
{
    public IActionResult Index() => View();
}
