using FoodEstablishment.Api.Data;
using FoodEstablishment.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace FoodEstablishment.Api.Repositories;

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