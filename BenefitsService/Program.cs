using BenefitsService.Data;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactApp", policy => policy
        .WithOrigins("http://localhost:5173", "http://localhost:3000")
        .AllowAnyHeader()
        .AllowAnyMethod());
});

builder.Services.AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Seed data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (!db.Categories.Any())
    {
        var halsa = new BenefitsService.Models.Category { Name = "Hälsa" };
        var friskvard = new BenefitsService.Models.Category { Name = "Friskvård" };
        var utbildning = new BenefitsService.Models.Category { Name = "Utbildning" };

        db.Categories.AddRange(halsa, friskvard, utbildning);

        db.Benefits.AddRange(
            new BenefitsService.Models.Benefit { Title = "Gymkort", Description = "Tillgång till gym och träningsanläggning", Value = 500, Category = halsa },
            new BenefitsService.Models.Benefit { Title = "Massage", Description = "Massagebehandling 60 min", Value = 800, Category = halsa },
            new BenefitsService.Models.Benefit { Title = "Friskvårdsbidrag", Description = "Årligt bidrag till friskvårdsaktiviteter", Value = 3000, Category = friskvard },
            new BenefitsService.Models.Benefit { Title = "Yogaklass", Description = "Tillgång till yogaklasser", Value = 400, Category = friskvard },
            new BenefitsService.Models.Benefit { Title = "Utbildningsbidrag", Description = "Bidrag till kurser och utbildningar", Value = 5000, Category = utbildning }
        );

        db.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseCors("ReactApp");

app.UseAuthorization();

app.MapControllers();

app.Run();