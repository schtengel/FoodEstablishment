namespace FoodEstablishment.Api.DTOs;

public record IngredientResponse
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Unit { get; init; } = string.Empty;
    public decimal StockQuantity { get; init; }
    public int StorageZoneId { get; init; }
}