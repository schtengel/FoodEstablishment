using FoodEstablishment.Api.Modules.Orders.Entities;
using FoodEstablishment.Api.Modules.Orders.Enums;
using FoodEstablishment.Api.Modules.Orders.Services;

namespace FoodEstablishment.Api.Modules.Orders.Repositories.Interfaces;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(int id);
    Task AddAsync(Order order);
    Task UpdateAsync(Order order);
    Task<bool> OrderSourceExistsAsync(int orderSourceId);
    Task<IEnumerable<Order>> GetUnpaidExpiredOrdersAsync(TimeSpan timeout);
    Task<IReadOnlyList<Order>> GetQueueAsync(IReadOnlyCollection<OrderStatusType> statuses);
    Task<MarkGivenResult> TryMarkGivenAsync(int id);
}