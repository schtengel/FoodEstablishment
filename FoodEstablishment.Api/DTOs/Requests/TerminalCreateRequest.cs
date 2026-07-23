namespace FoodEstablishment.Api.DTOs;

public record TerminalCreateRequest
{
    public string Username { get; init; } = string.Empty;
    public string DeviceId { get; init; } = string.Empty;
}