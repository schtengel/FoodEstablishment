namespace FoodEstablishment.Api.DTOs;

public class OrderItemCreateRequest
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}