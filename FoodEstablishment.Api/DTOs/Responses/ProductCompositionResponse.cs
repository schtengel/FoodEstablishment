namespace FoodEstablishment.Api.DTOs;

public record ProductCompositionResponse
{
    public int ProductId { get; init; }
    public int IngredientId { get; init; }
    public decimal Quantity { get; init; }
}