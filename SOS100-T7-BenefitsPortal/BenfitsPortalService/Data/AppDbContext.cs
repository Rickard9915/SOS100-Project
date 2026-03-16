using BenfitsPortal_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BenfitsPortal_Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<BudgetLimit> BudgetLimits => Set<BudgetLimit>();
}
