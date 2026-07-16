namespace FoodEstablishment.Api.DTOs;

public record OrderResponse
{
    public int Id { get; init; }
    public int UserId { get; init; }
    public int OrderSourceId { get; init; }
    public int OrderStatusId { get; init; }
    public DateTime CreatedAt { get; init; }
    public List<OrderCompositionResponse> Items { get; init; } = new();
    public List<ReceiptResponse> Receipts { get; init; } = new();
}