namespace BenefitsService.Models;

public class Benefit
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Value { get; set; }

    public int CategoryId { get; set; }
    public Category? Category { get; set; }
}
