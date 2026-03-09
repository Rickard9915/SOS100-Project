namespace SOS100_T7_BenefitsPortal.Models;

public class BenefitViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public int CategoryId { get; set; }
    public CategoryViewModel? Category { get; set; }
    public string CategoryName => Category?.Name ?? string.Empty;
}

public class CategoryViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class BenefitFormModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public int CategoryId { get; set; }
}

public class CategoryFormModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
