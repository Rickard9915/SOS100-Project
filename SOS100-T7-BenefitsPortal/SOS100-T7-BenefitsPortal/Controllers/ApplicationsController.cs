using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SOS100_T7_BenefitsPortal.Services;

namespace SOS100_T7_BenefitsPortal.Controllers;

public class ApplicationsController : Controller
{
    private readonly ApplicationService _applicationService;

    public ApplicationsController(ApplicationService applicationService)
    {
        _applicationService = applicationService;
    }

    public async Task<IActionResult> Index()
    {
        var applications = await _applicationService.GetApplicationsAsync();
        return View(applications);
    }

    public IActionResult Create(int? benefitId)
    {
        ViewBag.BenefitId = benefitId;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(string employeeName, int benefitId)
    {
        var application = new CreateApplicationDto
        {
            EmployeeName = employeeName,
            BenefitId = benefitId
        };

        await _applicationService.CreateApplicationAsync(application);

        return RedirectToAction("Index");
    }

    [HttpPost]
    [Authorize(Roles = "HR,Admin")]
    public async Task<IActionResult> Review(int id, string decision)
    {
        var reviewedBy = User.Identity?.Name ?? "HR";
        await _applicationService.ReviewApplicationAsync(id, decision, reviewedBy);
        return RedirectToAction("Index");
    }
}