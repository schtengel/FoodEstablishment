using FoodEstablishment.Api.Enums;

namespace FoodEstablishment.Api.Entities;

public class Receipt : BaseEntity
{
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
    
    public int PaymentStatusId { get; set; }
    public PaymentStatus PaymentStatus { get; set; } = null!;

    public PaymentMethodType PaymentMethod { get; set; }
    public DateTime? PaidAt { get; set; }
}