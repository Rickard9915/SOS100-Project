using System.Net.Http.Json;
using System.Text;
using BenfitsPortal_Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace BenfitsPortal_Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController(IHttpClientFactory httpClientFactory, IConfiguration configuration) : ControllerBase
{
    private string BenefitsUrl => configuration["ServiceUrls:BenefitsService"]!;
    private string ApplicationsUrl => configuration["ServiceUrls:ApplicationService"]!;

    // Statistik per förmån: antal ansökningar, godkända, avslagna, total kostnad
    [HttpGet("benefits")]
    public async Task<IActionResult> GetBenefitReport()
    {
        var http = httpClientFactory.CreateClient();

        var benefits = await http.GetFromJsonAsync<List<BenefitDto>>(BenefitsUrl) ?? [];
        var applications = await http.GetFromJsonAsync<List<ApplicationDto>>(ApplicationsUrl) ?? [];

        var report = benefits.Select(b =>
        {
            var apps = applications.Where(a => a.BenefitId == b.Id).ToList();
            var approved = apps.Where(a => a.Status == "Approved").ToList();

            return new BenefitReportItem
            {
                BenefitId = b.Id,
                BenefitName = b.Title,
                TotalApplications = apps.Count,
                ApprovedCount = approved.Count,
                RejectedCount = apps.Count(a => a.Status == "Rejected"),
                PendingCount = apps.Count(a => a.Status == "Pending"),
                TotalApprovedValue = approved.Count * b.Value
            };
        }).ToList();

        return Ok(report);
    }

    // Statistik per period (månad): antal ansökningar och kostnad per månad
    [HttpGet("applications")]
    public async Task<IActionResult> GetPeriodReport()
    {
        var http = httpClientFactory.CreateClient();

        var benefits = await http.GetFromJsonAsync<List<BenefitDto>>(BenefitsUrl) ?? [];
        var applications = await http.GetFromJsonAsync<List<ApplicationDto>>(ApplicationsUrl) ?? [];

        var benefitValues = benefits.ToDictionary(b => b.Id, b => b.Value);

        var report = applications
            .GroupBy(a => a.CreatedAt.ToString("yyyy-MM"))
            .OrderByDescending(g => g.Key)
            .Select(g =>
            {
                var approved = g.Where(a => a.Status == "Approved").ToList();
                return new PeriodReportItem
                {
                    Period = g.Key,
                    TotalApplications = g.Count(),
                    ApprovedCount = approved.Count,
                    RejectedCount = g.Count(a => a.Status == "Rejected"),
                    TotalApprovedValue = approved.Sum(a =>
                        benefitValues.TryGetValue(a.BenefitId, out var val) ? val : 0)
                };
            }).ToList();

        return Ok(report);
    }

    // Exporterar ansökningar som CSV-fil
    [HttpGet("export")]
    public async Task<IActionResult> ExportCsv(
        [FromQuery] string status = "Approved",
        [FromQuery] string? from = null,
        [FromQuery] string? to = null)
    {
        var http = httpClientFactory.CreateClient();

        var benefits = await http.GetFromJsonAsync<List<BenefitDto>>(BenefitsUrl) ?? [];
        var applications = await http.GetFromJsonAsync<List<ApplicationDto>>(ApplicationsUrl) ?? [];

        var benefitMap = benefits.ToDictionary(b => b.Id);

        var filtered = applications.Where(a => a.Status == status);

        if (from is not null && DateTime.TryParse(from + "-01", out var fromDate))
            filtered = filtered.Where(a => a.CreatedAt >= fromDate);

        if (to is not null && DateTime.TryParse(to + "-01", out var toDate))
            filtered = filtered.Where(a => a.CreatedAt <= toDate.AddMonths(1));

        var rows = filtered.Select(a => new ExportRow
        {
            ApplicationId = a.Id,
            EmployeeName = a.EmployeeName,
            BenefitName = benefitMap.TryGetValue(a.BenefitId, out var b) ? b.Title : "Okänd",
            Status = a.Status,
            BenefitValue = benefitMap.TryGetValue(a.BenefitId, out var bv) ? bv.Value : 0,
            CreatedAt = a.CreatedAt
        }).ToList();

        var csv = new StringBuilder();
        csv.AppendLine("AnsokningsId,Namn,Formån,Status,Belopp,Datum");
        foreach (var row in rows)
            csv.AppendLine($"{row.ApplicationId},{row.EmployeeName},{row.BenefitName},{row.Status},{row.BenefitValue},{row.CreatedAt:yyyy-MM-dd}");

        return File(Encoding.UTF8.GetBytes(csv.ToString()), "text/csv", "rapport.csv");
    }
}
