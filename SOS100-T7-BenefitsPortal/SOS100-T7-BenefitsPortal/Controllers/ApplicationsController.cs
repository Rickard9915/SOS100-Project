using Microsoft.AspNetCore.Mvc;

namespace SOS100_T7_BenefitsPortal.Controllers;

public class ApplicationsController : Controller
{
    public IActionResult Index() => View();

    public IActionResult Create(int? benefitId)
    {
        ViewBag.BenefitId = benefitId;
        return View();
    }
}
