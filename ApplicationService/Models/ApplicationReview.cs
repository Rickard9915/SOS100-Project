namespace ApplicationService.Models;

public class ApplicationReview
{
    public int Id { get; set; }

    public int ApplicationId { get; set; }
    public Application? Application { get; set; }

    public string ReviewedBy { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;
    public string Decision { get; set; } = string.Empty;
    public DateTime ReviewedAt { get; set; } = DateTime.UtcNow;
}