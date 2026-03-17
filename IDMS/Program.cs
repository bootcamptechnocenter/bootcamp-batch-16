using IDMS.Infrastructure.Data;
using IDMS.Middleware;
using IDMS.Modules.Master.Services;
using IDMS.Modules.Master.Services.Impl;
using IDMS.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// jwt
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtKey)),
        };
    });

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IMstBrandService, MstBrandService>();
builder.Services.AddScoped<IMstTypeService, MstTypeService>();
builder.Services.AddScoped<IMstUserService, MstUserService>();
builder.Services.AddScoped<IMstModelService, MstModelService>();
builder.Services.AddScoped<IMstStockService, MstStockService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Jwt Authorization header using the Bearer scheme.",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    option.AddSecurityDefinition("X-Api-Key", new OpenApiSecurityScheme
    {
        Description = "API Key Authorization header. Example: \"X-Api-Key: {your api key}\"",
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Name = "X-Api-Key",
    });

    option.AddSecurityRequirement(swaggerDoc => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference("Bearer", swaggerDoc, null),
            new List<string>()
        },
        {
            new OpenApiSecuritySchemeReference("X-Api-Key", swaggerDoc, null),
            new List<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// middleware
app.UseRequestMiddleware();

app.UseAuthentication();
app.UseMiddleware<ApiKeyMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
