using FoodEstablishment.Api.Modules.Menu.Entities;

namespace FoodEstablishment.Api.Modules.Menu.Repositories.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();
    
    Task AddAsync(Product product);
    
    Task<Product?> GetByIdAsync(int id);
    
    Task UpdateAsync(Product product);
}