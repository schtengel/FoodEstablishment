using FoodEstablishment.Api.Common.Entities;
using FoodEstablishment.Api.Modules.Identity.Entities;

namespace FoodEstablishment.Api.Modules.Orders.Entities;

public class Order : BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int OrderSourceId { get; set; }
    public OrderSource OrderSource { get; set; } = null!;

    public int OrderStatusId { get; set; }
    public OrderStatus OrderStatus { get; set; } = null!;

    public ICollection<OrderComposition> OrderCompositions { get; set; } = new List<OrderComposition>();
    public ICollection<Receipt> Receipts { get; set; } = new List<Receipt>();
}