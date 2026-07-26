namespace FoodEstablishment.Api.Modules.Identity.DTOs.Requests;

public record RegisterRequest
{
    public string Username { get; init; } = string.Empty;
    public string PhoneNumber { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}