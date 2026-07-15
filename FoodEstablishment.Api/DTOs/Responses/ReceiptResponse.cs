namespace FoodEstablishment.Api.DTOs;

public class ReceiptResponse
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int PaymentStatusId { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public DateTime? PaidAt { get; set; }
    public DateTime CreatedAt { get; set; }
}