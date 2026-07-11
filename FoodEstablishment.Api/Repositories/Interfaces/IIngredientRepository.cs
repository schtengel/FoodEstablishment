using FoodEstablishment.Api.Entities;

namespace FoodEstablishment.Api.Repositories;

public interface IIngredientRepository
{
    Task<IEnumerable<Ingredient>> GetAllAsync();
    
    Task AddAsync(Ingredient ingredient);
    
    Task<Ingredient?> GetByIdAsync(int id);
    
    Task UpdateAsync(Ingredient ingredient);
}