using System.ComponentModel.DataAnnotations;

namespace FoodEstablishment.Api.DTOs;

public class StorageZoneCreateRequest
{
    public string Name { get; set; } = string.Empty;
    
    public decimal RecommendedTemperature { get; set; }
}