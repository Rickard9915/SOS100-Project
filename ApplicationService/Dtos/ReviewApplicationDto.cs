namespace ApplicationService.Dtos;

public class ReviewApplicationDto
{
    public string ReviewedBy { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;
    public string Decision { get; set; } = string.Empty;
}