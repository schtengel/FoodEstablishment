using FoodEstablishment.Api.Data;
using FoodEstablishment.Api.Modules.Inventory.Entities;
using FoodEstablishment.Api.Modules.Inventory.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FoodEstablishment.Api.Modules.Inventory.Repositories;

public class SqlIngredientRepository(ApplicationDbContext context) : IIngredientRepository
{
    public async Task<IEnumerable<Ingredient>> GetAllAsync()
    {
        return await context.Ingredients
            .ToListAsync();
    }

    public async Task AddAsync(Ingredient ingredient)
    {
        await context.Ingredients.AddAsync(ingredient);
        await context.SaveChangesAsync();
    }

    public async Task<Ingredient?> GetByIdAsync(int id)
    {
        return await context.Ingredients.FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task UpdateAsync(Ingredient ingredient)
    {
        ingredient.UpdatedAt = DateTime.UtcNow;
        
        context.Ingredients.Update(ingredient);
        await context.SaveChangesAsync();
    }
}