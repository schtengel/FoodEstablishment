using System.ComponentModel.DataAnnotations;
using FoodEstablishment.Api.Enums;

namespace FoodEstablishment.Api.DTOs;

public class ProductCreateRequest
{
    [Required(ErrorMessage = "Название продукта обязательно.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Название должно быть от 2 до 100 символов.")]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(500, ErrorMessage = "Описание не должно превышать 500 символов.")]
    public string Description { get; set; } = string.Empty;
    
    [Range(1, 10000, ErrorMessage = "Вес или объем должен быть от 1 до 10 000.")]
    public int VolumeOrWeight { get; set; }
    
    [EnumDataType(typeof(UnitType), ErrorMessage = "Указана неверная единица измерения")]
    public UnitType Unit { get; set; }
    
    [Range(0, 5000, ErrorMessage = "Калорийность должна быть от 0 до 5000 ккал.")]
    public int Calories { get; set; }
    
    [Range(0.01, 100000.00, ErrorMessage = "Цена должна быть больше нуля.")]
    public decimal Price { get; set; }
    
    [Required(ErrorMessage = "Необходимо указать категорию")]
    public int CategoryId { get; set; }

    public bool IsStopListed { get; set; } = false;
}