namespace FoodEstablishment.Api.Modules.Menu.DTOs.Requests;

public record ProductCompositionCreateRequest
{
    public int ProductId { get; init; }
    public int IngredientId { get; init; }
    public decimal Quantity { get; init; }
}