namespace FoodEstablishment.Api.Modules.Identity.DTOs.Responses;

public record TerminalResponse
{
    public int Id { get; init; }
    public string Username { get; init; } = string.Empty;
    public string DeviceId { get; init; } = string.Empty;
}