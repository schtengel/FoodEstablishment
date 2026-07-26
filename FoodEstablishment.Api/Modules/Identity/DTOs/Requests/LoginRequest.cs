namespace FoodEstablishment.Api.Modules.Identity.DTOs.Requests;

public record LoginRequest
{
    public string PhoneNumber { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}