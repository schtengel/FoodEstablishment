using FoodEstablishment.Api.Data;
using FoodEstablishment.Api.Modules.Menu.Entities;
using FoodEstablishment.Api.Modules.Menu.Repositories.Interfaces;
using FoodEstablishment.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FoodEstablishment.Api.Modules.Menu.Repositories;

public class SqlCategoryRepository(ApplicationDbContext context) : ICategoryRepository
{
    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await context.Categories
            .ToListAsync();
    }

    public async Task AddAsync(Category category)
    {
        await context.Categories.AddAsync(category);
        await context.SaveChangesAsync();
    }
    
    public async Task<Category?> GetByIdAsync(int id)
    {
        return await context.Categories.FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task UpdateAsync(Category category)
    {
        category.UpdatedAt = DateTime.UtcNow;
        
        context.Categories.Update(category);
        await context.SaveChangesAsync();
    }
}