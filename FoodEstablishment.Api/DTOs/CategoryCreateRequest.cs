using System.ComponentModel.DataAnnotations;

namespace FoodEstablishment.Api.DTOs;

public class CategoryCreateRequest
{
    public string Name { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
}