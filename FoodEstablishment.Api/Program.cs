using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using FoodEstablishment.Api.Data;
using FoodEstablishment.Api.Modules.Identity.Repositories;
using FoodEstablishment.Api.Modules.Identity.Repositories.Interfaces;
using FoodEstablishment.Api.Modules.Identity.Services;
using FoodEstablishment.Api.Modules.Inventory.Repositories;
using FoodEstablishment.Api.Modules.Inventory.Repositories.Interfaces;
using FoodEstablishment.Api.Modules.Menu.DTOs.Validators;
using FoodEstablishment.Api.Modules.Menu.Repositories;
using FoodEstablishment.Api.Modules.Menu.Repositories.Interfaces;
using FoodEstablishment.Api.Modules.Orders.Repositories;
using FoodEstablishment.Api.Modules.Orders.Repositories.Interfaces;
using FoodEstablishment.Api.Modules.Orders.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
const string DevCorsPolicy = "DevCors";

// Add services to the container.

builder.Services.AddProblemDetails();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection"); // Строка подключения из appsettings.Development.json

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddControllers();
builder.Services.AddScoped<ICategoryRepository, SqlCategoryRepository>();
builder.Services.AddScoped<IStorageZoneRepository, SqlStorageZoneRepository>();
builder.Services.AddScoped<IProductRepository, SqlProductRepository>();
builder.Services.AddScoped<IUserRepository, SqlUserRepository>();
builder.Services.AddScoped<IIngredientRepository, SqlIngredientRepository>();
builder.Services.AddScoped<IProductCompositionRepository, SqlProductCompositionRepository>();
builder.Services.AddScoped<IOrderRepository, SqlOrderRepository>();
builder.Services.AddScoped<IReceiptRepository, SqlReceiptRepository>();

builder.Services.AddSingleton<TokenService>();
builder.Services.AddHostedService<OrderAutoCancellationService>();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<ProductCreateRequestValidator>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(DevCorsPolicy, policy =>
    {
        policy
            .SetIsOriginAllowed(origin => new Uri(origin).IsLoopback)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

var jwtSecret = builder.Configuration["JwtSettings:Secret"] ?? throw new InvalidOperationException("JWT Secret is missing");
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
        };
    });

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
app.UseCors(DevCorsPolicy);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();