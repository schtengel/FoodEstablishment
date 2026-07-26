using FoodEstablishment.Api.Common.Entities;

namespace FoodEstablishment.Api.Modules.Orders.Entities;

public class OrderSource : BaseEntity
{
    public string Name { get; set; } =  string.Empty;
}