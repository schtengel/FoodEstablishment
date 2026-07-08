namespace FoodEstablishment.Api.DTOs;

public class IngredientResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public decimal StockQuantity { get; set; }
    public int StorageZoneId { get; set; }
}