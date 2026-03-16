namespace BenfitsPortal_Api.Models;

// Budgettak per förmån och år lagras i lokal SQLite-databas
public class BudgetLimit
{
    public int Id { get; set; }
    public int BenefitId { get; set; }
    public string BenefitName { get; set; } = string.Empty;
    public decimal MaxAmount { get; set; }
    public int Year { get; set; }
}
