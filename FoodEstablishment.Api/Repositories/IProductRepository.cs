using FoodEstablishment.Api.Entities;

namespace FoodEstablishment.Api.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();
    
    Task AddAsync(Product product);
    
    Task<Product?> GetByIdAsync(int id);
    
    Task UpdateAsync(Product product);
}