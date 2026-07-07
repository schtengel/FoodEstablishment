using System.ComponentModel.DataAnnotations;

namespace FoodEstablishment.Api.DTOs;

public class StorageZoneCreateRequest
{
    [Required(ErrorMessage = "Название зоны хранения обязательно.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Название должно быть от 3 до 50 символов.")]
    public string Name { get; set; } = string.Empty;
    
    [Range(-50, 100, ErrorMessage = "Температура должна быть в диапазоне от -50 до 100 градусов")]
    public decimal RecommendedTemperature { get; set; }
}