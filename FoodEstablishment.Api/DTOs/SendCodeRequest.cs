using System.ComponentModel.DataAnnotations;

namespace FoodEstablishment.Api.DTOs;

public class SendCodeRequest
{
    [Required(ErrorMessage = "Номер телефона обязателен.")]
    [Phone(ErrorMessage = "Неверный формат номера телефона")]
    public string PhoneNumber { get; set; } = string.Empty;
}