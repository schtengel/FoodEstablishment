using FoodEstablishment.Api.Entities;

namespace FoodEstablishment.Api.Repositories;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(int id);
    Task AddAsync(Order order);
    Task UpdateAsync(Order order);
    Task<bool> OrderSourceExistsAsync(int orderSourceId);
    Task<IEnumerable<Order>> GetUnpaidExpiredOrdersAsync(TimeSpan timeout);
}