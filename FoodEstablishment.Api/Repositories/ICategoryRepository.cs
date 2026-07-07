using FoodEstablishment.Api.Entities;

namespace FoodEstablishment.Api.Repositories;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllAsync();
    
    Task AddAsync(Category category);
}