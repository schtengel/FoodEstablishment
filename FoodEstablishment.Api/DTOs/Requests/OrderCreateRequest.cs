namespace FoodEstablishment.Api.DTOs;

public class OrderCreateRequest
{
    public int UserId { get; set; }
    public int OrderSourceId { get; set; }
    
    public List<OrderItemCreateRequest> Items { get; set; } = new();
}