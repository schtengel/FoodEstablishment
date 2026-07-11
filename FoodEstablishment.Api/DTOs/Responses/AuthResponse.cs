namespace FoodEstablishment.Api.DTOs;

public class AuthResponse
{
    public string Token { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public int BonusPoints { get; set; }
}