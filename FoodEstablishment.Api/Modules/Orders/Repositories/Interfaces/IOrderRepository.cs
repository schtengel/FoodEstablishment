using FoodEstablishment.Api.Modules.Orders.Entities;

namespace FoodEstablishment.Api.Modules.Orders.Repositories.Interfaces;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(int id);
    Task AddAsync(Order order);
    Task UpdateAsync(Order order);
    Task<bool> OrderSourceExistsAsync(int orderSourceId);
    Task<IEnumerable<Order>> GetUnpaidExpiredOrdersAsync(TimeSpan timeout);
}