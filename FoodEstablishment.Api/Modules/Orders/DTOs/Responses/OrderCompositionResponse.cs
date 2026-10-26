namespace FoodEstablishment.Api.Modules.Orders.DTOs.Responses;

public record OrderCompositionResponse
{
    public int ProductId { get; init; }
    public string ProductName { get; init; } = string.Empty;
    public int Quantity { get; init; }
    public decimal PriceAtOrderTime { get; init; }
}