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

builder.Services.AddOpenApi();

// Все зависимости регистрируем до вызова
var app = builder.Build();

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();