using FoodEstablishment.Api.Common.Entities;

namespace FoodEstablishment.Api.Modules.Orders.Entities;

public class PaymentStatus : BaseEntity
{
    public string Name { get; set; } = string.Empty;
}