using FoodEstablishment.Api.Data;
using FoodEstablishment.Api.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddProblemDetails();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection"); // Строка подключения из appsettings.Development.json

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddControllers();
builder.Services.AddScoped<ICategoryRepository, SqlCategoryRepository>();
builder.Services.AddScoped<IStorageZoneRepository, SqlStorageZoneRepository>();
builder.Services.AddScoped<IProductRepository, SqlProductRepository>();

builder.Services.AddOpenApi();

// Все зависимости регистрируем до вызова
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("openapi/v1.json", "FoodEstablishment API v1");
        
        options.RoutePrefix = string.Empty;
    });
}

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();