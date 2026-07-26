namespace FoodEstablishment.Api.Modules.Identity.DTOs.Requests;

public record GuestSessionRequest
{
    public string? DeviceId { get; init; } = string.Empty;
}