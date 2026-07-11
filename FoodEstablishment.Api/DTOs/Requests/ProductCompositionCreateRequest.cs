namespace FoodEstablishment.Api.DTOs;

public class ProductCompositionCreateRequest
{
    public int ProductId { get; set; }
    public int IngredientId { get; set; }
    public decimal Quantity { get; set; }
}