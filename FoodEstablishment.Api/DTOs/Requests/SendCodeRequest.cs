namespace FoodEstablishment.Api.DTOs;

public record SendCodeRequest
{
    public string PhoneNumber { get; init; } = string.Empty;
}