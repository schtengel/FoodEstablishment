namespace FoodEstablishment.Api.Modules.Orders.DTOs.Requests;

public record OrderItemCreateRequest
{
    public int ProductId { get; init; }
    public int Quantity { get; init; }
}