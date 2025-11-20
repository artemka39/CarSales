using CarSales.Application.Interfaces;
using CarSales.Application.Services;
using CarSales.Infrastructure;
using CarSales.Contracts.Dtos;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<DealerDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IExcelService, ExcelService>();
builder.Services.AddScoped<ICarBrandService, CarBrandService>();
builder.Services.AddScoped<ICarModelService, CarModelService>();
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ITestDataService, TestDataService>();

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DealerDbContext>();
    db.Database.Migrate();
}

app.MapControllers();

app.Run();