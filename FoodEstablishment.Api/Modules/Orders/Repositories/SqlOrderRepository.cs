using FoodEstablishment.Api.Data;
using FoodEstablishment.Api.Modules.Inventory.Entities;
using FoodEstablishment.Api.Modules.Orders.Entities;
using FoodEstablishment.Api.Modules.Orders.Enums;
using FoodEstablishment.Api.Modules.Orders.Repositories.Interfaces;
using FoodEstablishment.Api.Modules.Orders.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace FoodEstablishment.Api.Modules.Orders.Repositories;

public class SqlOrderRepository(ApplicationDbContext context) : IOrderRepository
{
    public async Task<Order?> GetByIdAsync(int id)
    {
        return await context.Orders
            .Include(o => o.OrderCompositions)
                .ThenInclude(oc => oc.Product)
                    .ThenInclude(oc => oc.ProductCompositions)
                        .ThenInclude(oc => oc.Ingredient)
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

    public async Task<MarkGivenResult> TryMarkGivenAsync(int id)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();

        if (!await LockRowAsync("orders", id, onlyNotDeleted: true))
            return new MarkGivenResult.NotFound();

        var order = await context.Orders.FirstOrDefaultAsync(o => o.Id == id);
        if (order == null)
            return new MarkGivenResult.NotFound();

        if (order.OrderStatusId != (int)OrderStatusType.Ready)
            return new MarkGivenResult.Rejected("Выдать можно только заказ в статусе \"Готов\".");

        var lines = await context.OrderCompositions
            .Where(line => line.OrderId == id)
            .Include(line => line.Product)
                .ThenInclude(product => product.ProductCompositions)
            .IgnoreQueryFilters()
            .ToListAsync();

        if (lines.Any(line => line.Product == null))
        {
            var missingProductId = lines.First(line => line.Product == null).ProductId;
            return new MarkGivenResult.Rejected(
                $"Продукт с Id = {missingProductId} не найден. Выдать заказ нельзя.");
        }

        var ingredientIds = lines
            .SelectMany(line => line.Product.ProductCompositions)
            .Select(part => part.IngredientId)
            .Distinct()
            .OrderBy(ingredientId => ingredientId)
            .ToArray();

        foreach (var ingredientId in ingredientIds)
        {
            if (!await LockRowAsync("ingredients", ingredientId, onlyNotDeleted: false))
            {
                return new MarkGivenResult.Rejected(
                    $"Ингредиент с Id = {ingredientId} отсутствует на складе. Выдать заказ нельзя.");
            }
        }

        var ingredients = await context.Ingredients
            .IgnoreQueryFilters()
            .Where(ingredient => ingredientIds.Contains(ingredient.Id))
            .ToDictionaryAsync(ingredient => ingredient.Id);

        switch (StockDeduction.Plan(lines, ingredients))
        {
            case StockDeductionPlan.MissingIngredient missing:
                return new MarkGivenResult.Rejected(
                    $"Ингредиент с Id = {missing.IngredientId} отсутствует на складе. Выдать заказ нельзя.");
            case StockDeductionPlan.Ready ready:
                StockDeduction.Apply(ready, ingredients);
                break;
            default:
                throw new InvalidOperationException("Неизвестный план списания склада.");
        }

        order.OrderStatusId = (int)OrderStatusType.Given;
        order.UpdatedAt = DateTime.UtcNow;
        await context.SaveChangesAsync();
        await transaction.CommitAsync();

        return new MarkGivenResult.Success();
    }

    private async Task<bool> LockRowAsync(string table, int id, bool onlyNotDeleted)
    {
        var connection = context.Database.GetDbConnection();
        if (connection.State != System.Data.ConnectionState.Open)
            await connection.OpenAsync();

        await using var command = connection.CreateCommand();
        command.Transaction = context.Database.CurrentTransaction?.GetDbTransaction();
        command.CommandText = onlyNotDeleted
            ? $"SELECT id FROM {table} WHERE id = @id AND isdeleted = false FOR UPDATE"
            : $"SELECT id FROM {table} WHERE id = @id FOR UPDATE";

        var parameter = command.CreateParameter();
        parameter.ParameterName = "id";
        parameter.Value = id;
        command.Parameters.Add(parameter);

        await using var reader = await command.ExecuteReaderAsync();
        return await reader.ReadAsync();
    }
}