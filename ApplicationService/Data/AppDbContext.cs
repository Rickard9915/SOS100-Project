using ApplicationService.Models;
using Microsoft.EntityFrameworkCore;

namespace ApplicationService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Application> Applications { get; set; }
    public DbSet<ApplicationReview> ApplicationReviews { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Application>()
            .HasMany(a => a.Reviews)
            .WithOne(r => r.Application)
            .HasForeignKey(r => r.ApplicationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}