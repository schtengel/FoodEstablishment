using FoodEstablishment.Api.Enums;

namespace FoodEstablishment.Api.DTOs;

public class ReceiptStatusUpdateRequest
{
    public PaymentStatusType PaymentStatus { get; set; }
}