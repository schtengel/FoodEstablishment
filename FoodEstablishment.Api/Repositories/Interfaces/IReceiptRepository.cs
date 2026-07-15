using FoodEstablishment.Api.Entities;

namespace FoodEstablishment.Api.Repositories;

public interface IReceiptRepository
{
    Task<Receipt?> GetByIdAsync(int id);
    Task<IEnumerable<Receipt>> GetByOrderIdAsync(int orderId);
    Task AddAsync(Receipt receipt);
    Task UpdateAsync(Receipt receipt);
}