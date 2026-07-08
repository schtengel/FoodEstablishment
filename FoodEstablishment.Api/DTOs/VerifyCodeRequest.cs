using System.ComponentModel.DataAnnotations;

namespace FoodEstablishment.Api.DTOs;

public class VerifyCodeRequest
{
    [Required(ErrorMessage = "Номер телефона обязателен.")]
    public string PhoneNumber { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Код подтверждения обязателен.")]
    [StringLength(4, ErrorMessage = "Введите код")]
    public string Code { get; set; } = string.Empty;
}