using FoodEstablishment.Api.Modules.Menu.Entities;

namespace FoodEstablishment.Api.Modules.Orders.Entities;

public class OrderComposition
{
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public int Quantity { get; set; }
    public decimal PriceAtOrderTime { get; set; }
}