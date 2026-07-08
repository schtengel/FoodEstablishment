using System.ComponentModel.DataAnnotations;

namespace FoodEstablishment.Api.DTOs;

public class LoginRequest
{
    [Required(ErrorMessage = "Номер телефона обязателен.")]
    public string PhoneNumber { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Пароль обязателен.")]
    public string Password { get; set; } = string.Empty;
}