using System.ComponentModel.DataAnnotations.Schema;
using FoodEstablishment.Api.Enums;

namespace FoodEstablishment.Api.Entities;

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
}