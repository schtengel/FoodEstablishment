using FoodEstablishment.Api.Data;
using FoodEstablishment.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace FoodEstablishment.Api.Repositories;

public class SqlReceiptRepository(ApplicationDbContext context) : IReceiptRepository
{
    public async Task<Receipt?> GetByIdAsync(int id)
    {
        return await context.Receipts
            .Include(r => r.Order)
            .ThenInclude(o => o.OrderCompositions)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<IEnumerable<Receipt>> GetByOrderIdAsync(int orderId)
    {
        return await context.Receipts
            .Where(r => r.OrderId == orderId)
            .ToListAsync();
    }

    public async Task AddAsync(Receipt receipt)
    {
        await context.Receipts.AddAsync(receipt);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Receipt receipt)
    {
        receipt.UpdatedAt = DateTime.UtcNow;
        context.Receipts.Update(receipt);
        await context.SaveChangesAsync();
    }
}