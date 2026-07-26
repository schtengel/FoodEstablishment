using FoodEstablishment.Api.Modules.Inventory.Entities;

namespace FoodEstablishment.Api.Modules.Inventory.Repositories.Interfaces;

public interface IIngredientRepository
{
    Task<IEnumerable<Ingredient>> GetAllAsync();
    
    Task AddAsync(Ingredient ingredient);
    
    Task<Ingredient?> GetByIdAsync(int id);
    
    Task UpdateAsync(Ingredient ingredient);
}