using Microsoft.AspNetCore.Authentication.Cookies;
using WebView.Services;
using WebView.Services.Impl;

var builder = WebApplication.CreateBuilder(args);

var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"] ?? throw new InvalidOperationException("API base URL is not configured.");
var apiKey = builder.Configuration["ApiSettings:ApiKey"] ?? throw new InvalidOperationException("API key is not configured.");

// Http pointing to Web API
builder.Services.AddHttpClient("WebApi", client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.DefaultRequestHeaders.Add("X-Api-Key", apiKey);
});

// Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Cookie authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";
        options.LogoutPath = "/Logout";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.Cookie.HttpOnly = true;
    });

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IAuthClientService, AuthClientService>();
builder.Services.AddScoped<IMstBrandClientService, MstBrandClientService>();
builder.Services.AddScoped<IMstTypeClientService, MstTypeClientService>();
builder.Services.AddScoped<IMstModelClientService, MstModelClientService>();
builder.Services.AddScoped<IMstStockClientService, MstStockClientService>();

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

app.UseHttpsRedirection();
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
