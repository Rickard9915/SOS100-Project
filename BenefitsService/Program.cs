using BenefitsService.Data;
using Microsoft.EntityFrameworkCore;

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

// 🔥 Swagger istället för OpenApi/Scalar
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Seed data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    if (!db.Categories.Any())
    {
        var halsa = new BenefitsService.Models.Category { Name = "Hälsa" };
        var friskvard = new BenefitsService.Models.Category { Name = "Friskvård" };
        var utbildning = new BenefitsService.Models.Category { Name = "Utbildning" };
        var transport = new BenefitsService.Models.Category { Name = "Transport" };
        var matDryck = new BenefitsService.Models.Category { Name = "Mat & Dryck" };
        var semester = new BenefitsService.Models.Category { Name = "Semester" };
        var elektronik = new BenefitsService.Models.Category { Name = "Elektronik" };

        db.Categories.AddRange(halsa, friskvard, utbildning, transport, matDryck, semester, elektronik);

        db.Benefits.AddRange(
            new BenefitsService.Models.Benefit { Title = "Gymkort", Description = "Tillgång till gym och träningsanläggning", Value = 500, Category = halsa },
            new BenefitsService.Models.Benefit { Title = "Massage", Description = "Massagebehandling 60 min", Value = 800, Category = halsa },
            new BenefitsService.Models.Benefit { Title = "Friskvårdsbidrag", Description = "Årligt bidrag till friskvårdsaktiviteter", Value = 3000, Category = friskvard },
            new BenefitsService.Models.Benefit { Title = "Yogaklass", Description = "Tillgång till yogaklasser", Value = 400, Category = friskvard },
            new BenefitsService.Models.Benefit { Title = "Utbildningsbidrag", Description = "Bidrag till kurser och utbildningar", Value = 5000, Category = utbildning },
            new BenefitsService.Models.Benefit { Title = "Cykelförmån", Description = "Bidrag till köp av cykel för pendling", Value = 3000, Category = transport },
            new BenefitsService.Models.Benefit { Title = "Kollektivtrafikkort", Description = "Månadskort för kollektivtrafik", Value = 1000, Category = transport },
            new BenefitsService.Models.Benefit { Title = "Lunchbidrag", Description = "Dagligt bidrag till lunch på jobbet", Value = 1500, Category = matDryck },
            new BenefitsService.Models.Benefit { Title = "Fruktkorg", Description = "Färsk frukt på kontoret varje vecka", Value = 200, Category = matDryck },
            new BenefitsService.Models.Benefit { Title = "Semesterbidrag", Description = "Extra bidrag för semester och resor", Value = 5000, Category = semester },
            new BenefitsService.Models.Benefit { Title = "Friskvårdsresa", Description = "Bidrag till hälsoresor och retreats", Value = 3000, Category = semester },
            new BenefitsService.Models.Benefit { Title = "Mobiltelefon", Description = "Tjänstmobiltelefon eller bidrag till privat telefon", Value = 4000, Category = elektronik },
            new BenefitsService.Models.Benefit { Title = "Datorutrustning", Description = "Bidrag till hemmakontor och utrustning", Value = 6000, Category = elektronik }
        );

        db.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("ReactApp");

app.UseAuthorization();

app.MapControllers();

app.Run();