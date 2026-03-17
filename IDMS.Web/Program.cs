using IDMS.Infrastructure.Data;
using IDMS.Web.Services;
using IDMS.Web.Services.Impl;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"];
// var apiKey = builder.Configuration["ApiSettings:ApiKey"];

// Configure HttpClient for API calls
builder.Services.AddHttpClient("IDMSApi", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"] ?? "");
    client.DefaultRequestHeaders.Add("X-API-KEY", builder.Configuration["ApiSettings:ApiKey"] ?? "");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

// Session configuration
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Cookies authentication configuration
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/auth/login";
        options.LogoutPath = "/auth/logout";
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    });

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAuthClientService, AuthClientService>();
builder.Services.AddScoped<IMstBrandsService, MstBrandsService>();
builder.Services.AddScoped<IMstTypesService, MstTypesService>();
builder.Services.AddScoped<IMstModelsService, MstModelsService>();
builder.Services.AddScoped<IMstStocksService, MstStocksService>();

// builder.Services.AddDbContext<AppDbContext>(options =>
//     options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// builder.Services.AddScoped<IMstBrandsService, MstBrandsService>();
// builder.Services.AddScoped<IMstTypesService, MstTypesService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
