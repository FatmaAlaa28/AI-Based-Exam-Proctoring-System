using ExamMonitoringWeb.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// ≈÷«›… œ⁄„ snake_case
//builder.Services.AddControllersWithViews()
//    .AddJsonOptions(options =>
//    {
//        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.SnakeCaseLower;
//        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
//        options.JsonSerializerOptions.IgnoreNullValues = false; // ·œ⁄„ «·ÕﬁÊ· «·«Œ Ì«—Ì…
//    });
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.SnakeCaseLower;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.IgnoreNullValues = false;
    });

builder.Services.AddHttpClient();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole<int>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// asmaa 
// Enable request body buffering
builder.Services.Configure<IISServerOptions>(options =>
{
    options.AllowSynchronousIO = true; // Required for some scenarios, but use cautiously
});
///
var app = builder.Build();


// asmaa
// Middleware to enable request body buffering
app.Use(async (context, next) =>
{
    context.Request.EnableBuffering();
    await next(context);
});

///
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

// app.UseHttpsRedirection(); // „⁄·¯ﬁ ⁄‘«‰  ” Œœ„Ì HTTP
app.UseStaticFiles();
app.UseSession();
app.UseAuthentication();
app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();