namespace FoodEstablishment.Api.Modules.Menu.DTOs.Responses;

public record ProductCompositionResponse
{
    public int ProductId { get; init; }
    public int IngredientId { get; init; }
    public decimal Quantity { get; init; }
}