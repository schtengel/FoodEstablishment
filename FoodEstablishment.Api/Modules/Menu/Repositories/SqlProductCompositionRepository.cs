using FoodEstablishment.Api.Data;
using FoodEstablishment.Api.Modules.Menu.Entities;
using FoodEstablishment.Api.Modules.Menu.Repositories.Interfaces;
using FoodEstablishment.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FoodEstablishment.Api.Modules.Menu.Repositories;

public class SqlProductCompositionRepository(ApplicationDbContext context) : IProductCompositionRepository
{
    public async Task<IEnumerable<ProductComposition>> GetByProductIdAsync(int productId)
    {
        return await context.ProductCompositions
            .Where(pc => pc.ProductId == productId)
            .ToListAsync();
    }

    public async Task<ProductComposition?> GetAsync(int productId, int ingredientId)
    {
        return await context.ProductCompositions
            .FirstOrDefaultAsync(pc => pc.ProductId == productId && pc.IngredientId == ingredientId);
    }

    public async Task AddAsync(ProductComposition composition)
    {
        await context.ProductCompositions.AddAsync(composition);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ProductComposition composition)
    {
        context.ProductCompositions.Update(composition);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(ProductComposition composition)
    {
        context.ProductCompositions.Remove(composition);
        await context.SaveChangesAsync();
    }
}