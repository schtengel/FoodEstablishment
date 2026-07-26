using FoodEstablishment.Api.Data;
using FoodEstablishment.Api.Modules.Menu.Entities;
using FoodEstablishment.Api.Modules.Menu.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FoodEstablishment.Api.Modules.Menu.Repositories;

public class SqlProductRepository(ApplicationDbContext context) : IProductRepository
{
    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await context.Products.ToListAsync();
    }

    public async Task AddAsync(Product product)
    {
        await context.Products.AddAsync(product);
        await context.SaveChangesAsync();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await context.Products.FirstOrDefaultAsync(p => p.Id == id);
    }

    public Task UpdateAsync(Product product)
    {
        product.UpdatedAt = DateTime.UtcNow;
        
        context.Products.Update(product);
        return context.SaveChangesAsync();
    }
}