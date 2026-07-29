
using FoodEstablishment.Api.Modules.Orders.Enums;

namespace FoodEstablishment.Api.Modules.Orders.DTOs.Requests;

public record OrderCreateRequest
{
    public int OrderSourceId { get; init; }
    public PaymentMethodType PaymentMethod { get; init; }
    public List<OrderItemCreateRequest> Items { get; init; } = new();
}