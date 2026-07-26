using FoodEstablishment.Api.Modules.Menu.Entities;

namespace FoodEstablishment.Api.Modules.Menu.Repositories.Interfaces;

public interface IProductCompositionRepository
{
    Task<IEnumerable<ProductComposition>> GetByProductIdAsync(int productId);
    Task<ProductComposition?> GetAsync(int productId, int ingredientId);
    Task AddAsync(ProductComposition composition);
    Task UpdateAsync(ProductComposition composition);
    Task DeleteAsync(ProductComposition composition);
}