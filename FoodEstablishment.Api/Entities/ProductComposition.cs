namespace FoodEstablishment.Api.Entities;

public class ProductComposition
{
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    
    public int IngredientId { get; set; }
    public Ingredient Ingredient { get; set; } = null!;
    
    public decimal Quantity { get; set; }
    
}