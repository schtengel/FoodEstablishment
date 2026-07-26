namespace FoodEstablishment.Api.Modules.Identity.DTOs.Requests;

public record SendCodeRequest
{
    public string PhoneNumber { get; init; } = string.Empty;
}