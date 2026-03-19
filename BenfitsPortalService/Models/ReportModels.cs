namespace BenfitsPortal_Api.Models;

public class BenefitDto
{
    public int Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public decimal Value { get; init; }
    public int CategoryId { get; init; }
}

public class ApplicationDto
{
    public int Id { get; init; }
    public string EmployeeName { get; init; } = string.Empty;
    public int BenefitId { get; init; }
    public string Status { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
}

public class BenefitReportItem
{
    public int BenefitId { get; init; }
    public string BenefitName { get; init; } = string.Empty;
    public int TotalApplications { get; init; }
    public int ApprovedCount { get; init; }
    public int RejectedCount { get; init; }
    public int PendingCount { get; init; }
    public decimal TotalApprovedValue { get; init; }
}

public class PeriodReportItem
{
    public string Period { get; init; } = string.Empty;
    public int TotalApplications { get; init; }
    public int ApprovedCount { get; init; }
    public int RejectedCount { get; init; }
    public decimal TotalApprovedValue { get; init; }
}

public class ExportRow
{
    public int ApplicationId { get; init; }
    public string EmployeeName { get; init; } = string.Empty;
    public string BenefitName { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public decimal BenefitValue { get; init; }
    public DateTime CreatedAt { get; init; }
}
