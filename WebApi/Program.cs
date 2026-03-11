using Microsoft.EntityFrameworkCore;
using WebApi.Infrastructure.Data;
using WebApi.Modules.Master.Services;
using WebApi.Modules.Master.Services.Impl;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<IMstBrandService, MstBrandService>();
builder.Services.AddScoped<IMstTypeService, MstTypeService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
