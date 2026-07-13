namespace FoodEstablishment.Api.DTOs;

public class OrderCompositionResponse
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal PriceAtOrderTime { get; set; }
}