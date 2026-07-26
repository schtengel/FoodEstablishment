using FoodEstablishment.Api.Modules.Orders.Entities;

namespace FoodEstablishment.Api.Modules.Orders.Repositories.Interfaces;

public interface IReceiptRepository
{
    Task<Receipt?> GetByIdAsync(int id);
    Task<IEnumerable<Receipt>> GetByOrderIdAsync(int orderId);
    Task AddAsync(Receipt receipt);
    Task UpdateAsync(Receipt receipt);
}