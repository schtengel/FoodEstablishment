using System.ComponentModel.DataAnnotations;
using FoodEstablishment.Api.Enums;

namespace FoodEstablishment.Api.DTOs;

public class ProductCreateRequest
{
    public string Name { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    public int VolumeOrWeight { get; set; }

    public UnitType Unit { get; set; }

    public int Calories { get; set; }

    public decimal Price { get; set; }

    public int CategoryId { get; set; }

    public bool IsStopListed { get; set; } = false;
}