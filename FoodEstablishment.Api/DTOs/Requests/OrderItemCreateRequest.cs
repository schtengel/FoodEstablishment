namespace FoodEstablishment.Api.DTOs;

public record OrderItemCreateRequest
{
    public int ProductId { get; init; }
    public int Quantity { get; init; }
}