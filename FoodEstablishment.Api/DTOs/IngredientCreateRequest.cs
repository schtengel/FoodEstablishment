using System.ComponentModel.DataAnnotations;

namespace FoodEstablishment.Api.DTOs;

public class IngredientCreateRequest
{
    [Required(ErrorMessage = "Название ингредиента обязательно.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Название должно быть от 2 до 100 символов.")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Единица измерения обязательна.")]
    [StringLength(10, ErrorMessage = "Единица измерения не должна превышать 10 символов.")]
    public string Unit { get; set; } = string.Empty;
    
    [Range(0.000, 1_000_000.000, ErrorMessage = "Количество на складе не может быть отрицательным.")]
    public decimal StockQuantity { get; set; }
    
    [Required(ErrorMessage = "Необходимо указать зону хранения.")]
    public int StorageZoneId { get; set; }
}