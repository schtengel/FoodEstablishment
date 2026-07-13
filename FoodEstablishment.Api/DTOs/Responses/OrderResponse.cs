namespace FoodEstablishment.Api.DTOs;

public class OrderResponse
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int OrderSourceId { get; set; }
    public int OrderStatusId { get; set; }
    public DateTime CreatedAt { get; set; }

    public List<OrderCompositionResponse> Items { get; set; } = new();
}