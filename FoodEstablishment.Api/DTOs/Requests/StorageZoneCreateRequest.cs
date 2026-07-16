namespace FoodEstablishment.Api.DTOs;

public record StorageZoneCreateRequest
{
    public string Name { get; init; } = string.Empty;
    public decimal RecommendedTemperature { get; init; }
}