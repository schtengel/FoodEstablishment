using FoodEstablishment.Api.Common.Entities;

namespace FoodEstablishment.Api.Modules.Orders.Entities;

public class OrderStatus : BaseEntity
{
    public string Name { get; set; } = string.Empty;
}