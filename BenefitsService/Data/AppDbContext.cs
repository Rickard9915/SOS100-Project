using BenefitsService.Models;
using Microsoft.EntityFrameworkCore;

namespace BenefitsService.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Benefit> Benefits => Set<Benefit>();
}
