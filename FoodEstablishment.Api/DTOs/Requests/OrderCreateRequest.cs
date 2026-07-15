using FoodEstablishment.Api.Enums;

namespace FoodEstablishment.Api.DTOs;

public class OrderCreateRequest
{
    public int OrderSourceId { get; set; }
    public PaymentMethodType PaymentMethod { get; set; }
    public List<OrderItemCreateRequest> Items { get; set; } = new();
}