using Microsoft.AspNetCore.Authentication.Cookies;
using SOS100_T7_BenefitsPortal.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient<BenefitService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ServiceUrls:BenefitsService"]!);
});

builder.Services.AddHttpClient<CategoryService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ServiceUrls:BenefitsService"]!);
});

builder.Services.AddHttpClient<ApplicationService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ServiceUrls:ApplicationService"]!);
});

builder.Services.AddHttpClient<ReportService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ServiceUrls:ReportService"]!);
});

builder.Services.AddHttpClient<UserApiService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ServiceUrls:UserService"]!);
});

builder.Services.AddHttpClient<NotificationApiService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ServiceUrls:NotificationService"]!);
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => options.LoginPath = "/Account/Login");

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
