using System.Text;
using IDMS.Common;
using IDMS.Infrastructure.Data;
using IDMS.Middleware;
using IDMS.Modules.Master.Services;
using IDMS.Modules.Master.Services.Impl;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

//jwt
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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var authorizationHeader = context.Request.Headers.Authorization.ToString();

                if (authorizationHeader.StartsWith("Bearer Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    context.Token = authorizationHeader["Bearer Bearer ".Length..].Trim();
                }

                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddScoped<IMstBrandService, MstBrandService>();
builder.Services.AddScoped<IMstTypeService, MstTypeService>();
builder.Services.AddScoped<IMstUserService, MstUserService>();
builder.Services.AddScoped<IMstModelService, MstModelService>();
builder.Services.AddScoped<IMstStockService, MstStockService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IJwtService, JwtServices>();

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    x => x.Key,
                    x => x.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            var response = new ApiResponse<object>
            {
                Status = false,
                Message = "Validation failed",
                Data = new
                {
                    Errors = errors,
                    TraceId = context.HttpContext.TraceIdentifier
                }
            };

            return new BadRequestObjectResult(response);
        };
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Jwt Authorization header using the Bearer scheme.",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
    });

    options.AddSecurityDefinition("X-Api-Key", new OpenApiSecurityScheme
    {
        Description = "API Key Authorization header. Example: \"X-Api-Key: {your api key}\"",
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Name = "X-Api-Key",
    });

    options.AddSecurityRequirement(swaggerDoc => new OpenApiSecurityRequirement
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

//middleware
app.UseRequestMiddleware();

app.UseMiddleware<ApiKeyMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
