namespace ApplicationService.Models;

public class Application
{
    public int Id { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public int BenefitId { get; set; }
    public string Status { get; set; } = "Pending";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<ApplicationReview> Reviews { get; set; } = [];
}