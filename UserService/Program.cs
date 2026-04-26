using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// DbContext 
builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Middleware
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("AllowReactApp");

app.MapControllers();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<UserDbContext>();
    db.Database.EnsureCreated();

    if (!db.Users.Any())
    {
        db.Users.AddRange(
            new User
            {
                Name = "Anställd Test",
                Email = "employee@test.com",
                PasswordHash = "Test123!",
                Role = "Employee"
            },
            new User
            {
                Name = "HR Test",
                Email = "hr@test.com",
                PasswordHash = "Test123!",
                Role = "HR"
            }
        );
        db.SaveChanges();
    }
}
app.Run();