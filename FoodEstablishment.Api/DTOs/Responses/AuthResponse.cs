namespace FoodEstablishment.Api.DTOs;

public record AuthResponse
{
    public string Token { get; init; } = string.Empty;
    public string Username { get; init; } = string.Empty;
    public int BonusPoints { get; init; }
}