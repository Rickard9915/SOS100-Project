using ApplicationService.Data;
using ApplicationService.Filters;
using ApplicationService.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=applications.db"));

builder.Services.AddScoped<ApiKeyFilter>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();

    if (!db.Applications.Any())
    {
        db.Applications.AddRange(
            new Application
            {
                EmployeeName = "Anställd Test",
                BenefitId = 1,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            },
            new Application
            {
                EmployeeName = "Anställd Test",
                BenefitId = 2,
                Status = "Approved",
                CreatedAt = DateTime.UtcNow.AddDays(-5)
            },
            new Application
            {
                EmployeeName = "HR Test",
                BenefitId = 1,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow.AddDays(-2)
            }
        );
        db.SaveChanges();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Tillfälligt borttagen för lokal testning mellan frontend och API
// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();