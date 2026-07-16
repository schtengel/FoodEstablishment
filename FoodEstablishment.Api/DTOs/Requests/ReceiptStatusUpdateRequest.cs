using FoodEstablishment.Api.Enums;

namespace FoodEstablishment.Api.DTOs;

public record ReceiptStatusUpdateRequest
{
    public PaymentStatusType PaymentStatus { get; init; }
}