using FoodEstablishment.Api.Modules.Orders.Enums;

namespace FoodEstablishment.Api.Modules.Orders.DTOs.Requests;

public record ReceiptStatusUpdateRequest
{
    public PaymentStatusType PaymentStatus { get; init; }
}