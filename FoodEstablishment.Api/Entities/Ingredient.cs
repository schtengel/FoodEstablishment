using System.ComponentModel.DataAnnotations.Schema;

namespace FoodEstablishment.Api.Entities;

public class Ingredient : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    
    [Column(TypeName = "decimal(18,3)")]
    public decimal StockQuantity { get; set; }
    
    public int StorageZoneId { get; set; }
    public StorageZone StorageZone { get; set; } = null!;
}