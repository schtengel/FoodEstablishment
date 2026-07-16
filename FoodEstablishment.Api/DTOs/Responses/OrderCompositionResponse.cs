namespace FoodEstablishment.Api.DTOs;

public record OrderCompositionResponse
{
    public int ProductId { get; init; }
    public int Quantity { get; init; }
    public decimal PriceAtOrderTime { get; init; }
}