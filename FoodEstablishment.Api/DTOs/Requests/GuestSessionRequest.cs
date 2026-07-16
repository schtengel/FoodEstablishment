namespace FoodEstablishment.Api.DTOs;

public record GuestSessionRequest
{
    public string? DeviceId { get; init; } = string.Empty;
}