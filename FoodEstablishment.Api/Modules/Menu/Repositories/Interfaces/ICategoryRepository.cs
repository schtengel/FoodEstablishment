using FoodEstablishment.Api.Modules.Menu.Entities;

namespace FoodEstablishment.Api.Modules.Menu.Repositories.Interfaces;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllAsync();
    
    Task AddAsync(Category category);
    
    Task<Category?> GetByIdAsync(int id);
    
    Task UpdateAsync(Category category);
}