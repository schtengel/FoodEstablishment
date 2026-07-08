using System.Linq.Expressions;
using FoodEstablishment.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace FoodEstablishment.Api.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    {
        
    }
    public DbSet<OrderStatus> OrderStatuses { get; set; } = null!;
    public DbSet<PaymentStatus> PaymentStatuses { get; set; } = null!;
    public DbSet<OrderSource> OrderSources { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<StorageZone> StorageZones { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Ingredient> Ingredients { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json")
                .Build();
            
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            
            optionsBuilder.UseNpgsql(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            var tableName = entity.GetTableName()?.ToLower();
            if (tableName != null)
                entity.SetTableName(tableName);

            foreach (var property in entity.GetProperties())
            {
                property.SetColumnName(property.Name.ToLower());
            }
            
            modelBuilder.Entity<Ingredient>()
                .HasOne(x => x.StorageZone)
                .WithMany(x => x.Ingredients)
                .HasForeignKey(x => x.StorageZoneId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Category>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<OrderStatus>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<PaymentStatus>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<OrderSource>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<StorageZone>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Product>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Ingredient>().HasQueryFilter(x => !x.IsDeleted);
        }
        
        var seedDate = new  DateTime(2026, 7, 7, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<OrderStatus>().HasData(
            new OrderStatus { Id = 1, Name = "Создан", CreatedAt = seedDate},
            new OrderStatus { Id = 2, Name = "В процессе", CreatedAt = seedDate},
            new OrderStatus { Id = 3, Name = "Готов", CreatedAt = seedDate},
            new OrderStatus { Id = 4, Name = "Отменен", CreatedAt = seedDate}
        );

        modelBuilder.Entity<PaymentStatus>().HasData(
            new PaymentStatus { Id = 1, Name = "Создан", CreatedAt = seedDate},
            new PaymentStatus { Id = 2, Name = "В процессе оплаты", CreatedAt = seedDate},
            new PaymentStatus { Id = 3, Name = "Оплачен", CreatedAt = seedDate},
            new PaymentStatus { Id = 4, Name = "Недостаточно средств", CreatedAt = seedDate},
            new PaymentStatus { Id = 5, Name = "Отменен", CreatedAt = seedDate}
        );

        modelBuilder.Entity<OrderSource>().HasData(
            new OrderSource { Id = 1, Name = "Мобильное приложение", CreatedAt = seedDate},
            new OrderSource { Id = 2, Name = "Терминал самообслуживания", CreatedAt = seedDate},
            new OrderSource { Id = 3, Name = "Касса", CreatedAt = seedDate},
            new OrderSource { Id = 4, Name = "Веб-сайт", CreatedAt = seedDate},
            new OrderSource { Id = 5, Name = "Другое", CreatedAt = seedDate}
        );
    }
}