using FoodEstablishment.Api.Data;
using FoodEstablishment.Api.Entities;
using FoodEstablishment.Api.Enums;
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
    
    public async Task UpdateAsync(Order order)
    {
        order.UpdatedAt = DateTime.UtcNow;
        context.Orders.Update(order);
        await context.SaveChangesAsync();
    }

    public async Task<bool> OrderSourceExistsAsync(int orderSourceId)
    {
        return await context.OrderSources.AnyAsync(os => os.Id == orderSourceId);
    }
    
    public async Task<IEnumerable<Order>> GetUnpaidExpiredOrdersAsync(TimeSpan timeout)
    {
        var cutoff = DateTime.UtcNow - timeout;

        return await context.Orders
            .Include(o => o.Receipts)
            .Where(o => o.OrderStatusId == (int)OrderStatusType.Created
                        && o.CreatedAt < cutoff
                        && o.Receipts.All(r => r.PaymentStatusId != (int)PaymentStatusType.Paid))
            .ToListAsync();
    }
}