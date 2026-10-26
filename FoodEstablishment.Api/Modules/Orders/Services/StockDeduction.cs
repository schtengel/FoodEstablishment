using FoodEstablishment.Api.Modules.Inventory.Entities;
using FoodEstablishment.Api.Modules.Orders.Entities;

namespace FoodEstablishment.Api.Modules.Orders.Services;

public static class StockDeduction
{
    public static StockDeductionPlan Plan(
        IEnumerable<OrderComposition> lines,
        IReadOnlyDictionary<int, Ingredient> ingredientsById)
    {
        var required = new Dictionary<int, decimal>();

        foreach (var line in lines)
        {
            foreach (var part in line.Product.ProductCompositions)
            {
                var amount = part.Quantity * line.Quantity;
                if (required.TryGetValue(part.IngredientId, out var current))
                    required[part.IngredientId] = current + amount;
                else
                    required[part.IngredientId] = amount;
            }
        }

        var ready = new List<StockDeductionLine>(required.Count);

        foreach (var (ingredientId, amount) in required.OrderBy(pair => pair.Key))
        {
            if (!ingredientsById.TryGetValue(ingredientId, out var ingredient) || ingredient.IsDeleted)
                return new StockDeductionPlan.MissingIngredient(ingredientId);

            ready.Add(new StockDeductionLine(ingredientId, amount));
        }

        return new StockDeductionPlan.Ready(ready);
    }

    public static void Apply(StockDeductionPlan.Ready plan, IReadOnlyDictionary<int, Ingredient> ingredientsById)
    {
        foreach (var line in plan.Lines)
        {
            var ingredient = ingredientsById[line.IngredientId];
            ingredient.StockQuantity -= line.Quantity;
            ingredient.UpdatedAt = DateTime.UtcNow;
        }
    }
}

public readonly record struct StockDeductionLine(int IngredientId, decimal Quantity);

public abstract record StockDeductionPlan
{
    private StockDeductionPlan()
    {
    }

    public sealed record Ready(IReadOnlyList<StockDeductionLine> Lines) : StockDeductionPlan;

    public sealed record MissingIngredient(int IngredientId) : StockDeductionPlan;
}