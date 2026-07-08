using System.ComponentModel.DataAnnotations;

namespace FoodEstablishment.Api.DTOs;

public class RegisterRequest
{
    [Required(ErrorMessage = "Имя пользователя обязательно.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Имя пользователя должно быть от 3 до 50 символов.")]
    public string Username { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Номер телефона обязателен.")]
    [Phone(ErrorMessage = "Неверный формат номера телефона.")]
    public string PhoneNumber { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Пароль обязателен.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль должен быть не менее 6 символов.")]
    public string Password { get; set; } = string.Empty;
}