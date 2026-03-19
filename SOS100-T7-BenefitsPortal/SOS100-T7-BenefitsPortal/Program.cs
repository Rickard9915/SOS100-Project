using SOS100_T7_BenefitsPortal.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient<BenefitService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5062/");
});

builder.Services.AddHttpClient<CategoryService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5062/");
});

builder.Services.AddHttpClient<ApplicationService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5237/");
});

var app = builder.Build();


// Configure the HTTP request pipeline.
// if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();