namespace FoodEstablishment.Api.Modules.Identity.DTOs.Requests;

public record TerminalCreateRequest
{
    public string Username { get; init; } = string.Empty;
    public string DeviceId { get; init; } = string.Empty;
}