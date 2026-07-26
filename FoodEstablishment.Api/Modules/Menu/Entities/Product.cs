using System.ComponentModel.DataAnnotations.Schema;
using FoodEstablishment.Api.Common.Entities;
using FoodEstablishment.Api.Modules.Menu.Enums;
using FoodEstablishment.Api.Modules.Orders.Entities;

namespace FoodEstablishment.Api.Modules.Menu.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }
    
    public int VolumeOrWeight { get; set; }
    public UnitType Unit { get; set; }
    public int Calories { get; set; }
    public bool IsStopListed { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    
    public ICollection<ProductComposition> ProductCompositions { get; set; } = new List<ProductComposition>();
    public ICollection<OrderComposition> OrderCompositions { get; set; } = new List<OrderComposition>();
}