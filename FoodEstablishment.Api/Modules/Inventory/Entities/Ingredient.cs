using System.ComponentModel.DataAnnotations.Schema;
using FoodEstablishment.Api.Common.Entities;
using FoodEstablishment.Api.Modules.Menu.Entities;

namespace FoodEstablishment.Api.Modules.Inventory.Entities;

public class Ingredient : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    
    [Column(TypeName = "decimal(18,3)")]
    public decimal StockQuantity { get; set; }
    
    public int StorageZoneId { get; set; }
    public StorageZone StorageZone { get; set; } = null!;
    
    public ICollection<ProductComposition> ProductCompositions { get; set; } = new List<ProductComposition>();
}