using FoodEstablishment.Api.Data;
using FoodEstablishment.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace FoodEstablishment.Api.Repositories;

public class SqlOrderRepository(ApplicationDbContext context) : IOrderRepository
{
    public async Task<Order?> GetByIdAsync(int id)
    {
        return await context.Orders
            .Include(o => o.OrderCompositions)
            .Include(o => o.Receipts)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task AddAsync(Order order)
    {
        await context.Orders.AddAsync(order);
        await context.SaveChangesAsync();
    }

    public async Task<bool> OrderSourceExistsAsync(int orderSourceId)
    {
        return await context.OrderSources.AnyAsync(os => os.Id == orderSourceId);
    }
}