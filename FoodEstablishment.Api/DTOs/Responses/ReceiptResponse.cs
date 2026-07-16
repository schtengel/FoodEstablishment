namespace FoodEstablishment.Api.DTOs;

public record ReceiptResponse
{
    public int Id { get; init; }
    public int OrderId { get; init; }
    public int PaymentStatusId { get; init; }
    public string PaymentMethod { get; init; } = string.Empty;
    public decimal TotalAmount { get; init; }
    public DateTime? PaidAt { get; init; }
    public DateTime CreatedAt { get; init; }
}