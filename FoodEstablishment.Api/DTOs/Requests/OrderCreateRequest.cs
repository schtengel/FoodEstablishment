using FoodEstablishment.Api.Enums;

namespace FoodEstablishment.Api.DTOs;

public record OrderCreateRequest
{
    public int OrderSourceId { get; init; }
    public PaymentMethodType PaymentMethod { get; init; }
    public List<OrderItemCreateRequest> Items { get; init; } = new();
}