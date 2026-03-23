using System.Net.Http.Json;

namespace SOS100_T7_BenefitsPortal.Services;

public class ReportService
{
    private readonly HttpClient _httpClient;

    public ReportService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<BenefitReportItem>> GetBenefitReportAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<BenefitReportItem>>("api/reports/benefits")
               ?? new List<BenefitReportItem>();
    }

    public async Task<List<PeriodReportItem>> GetPeriodReportAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<PeriodReportItem>>("api/reports/applications")
               ?? new List<PeriodReportItem>();
    }
}

public class BenefitReportItem
{
    public int BenefitId { get; set; }
    public string BenefitName { get; set; } = "";
    public int TotalApplications { get; set; }
    public int ApprovedCount { get; set; }
    public int RejectedCount { get; set; }
    public int PendingCount { get; set; }
    public decimal TotalApprovedValue { get; set; }
}

public class PeriodReportItem
{
    public string Period { get; set; } = "";
    public int TotalApplications { get; set; }
    public int ApprovedCount { get; set; }
    public int RejectedCount { get; set; }
    public decimal TotalApprovedValue { get; set; }
}
