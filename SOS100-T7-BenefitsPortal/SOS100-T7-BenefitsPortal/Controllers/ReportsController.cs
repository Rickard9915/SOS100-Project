using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SOS100_T7_BenefitsPortal.Services;

namespace SOS100_T7_BenefitsPortal.Controllers;

[Authorize(Roles = "HR,Admin,Finance")]
public class ReportsController : Controller
{
    private readonly ReportService _reportService;

    public ReportsController(ReportService reportService)
    {
        _reportService = reportService;
    }

    public async Task<IActionResult> Index()
    {
        var benefitReport = await _reportService.GetBenefitReportAsync();
        var periodReport = await _reportService.GetPeriodReportAsync();

        ViewBag.BenefitReport = benefitReport;
        ViewBag.PeriodReport = periodReport;

        return View();
    }
}
