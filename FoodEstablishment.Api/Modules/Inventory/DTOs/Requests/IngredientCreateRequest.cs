namespace FoodEstablishment.Api.Modules.Inventory.DTOs.Requests;

public record IngredientCreateRequest
{
    public string Name { get; init; } = string.Empty;
    public string Unit { get; init; } = string.Empty;
    public decimal StockQuantity { get; init; }
    public int StorageZoneId { get; init; }
}