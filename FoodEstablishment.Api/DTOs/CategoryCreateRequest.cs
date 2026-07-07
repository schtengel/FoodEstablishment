using System.ComponentModel.DataAnnotations;

namespace FoodEstablishment.Api.DTOs;

public class CategoryCreateRequest
{
    [Required(ErrorMessage = "Название категории обязательно для заполнения.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Название должно быть от 3 до 50 символов.")]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(200, ErrorMessage = "Описание не должно превышать 200 символов.")]
    public string Description { get; set; } = string.Empty;
}