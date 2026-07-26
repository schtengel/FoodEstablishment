namespace FoodEstablishment.Api.Modules.Inventory.DTOs.Requests;

public record StorageZoneCreateRequest
{
    public string Name { get; init; } = string.Empty;
    public decimal RecommendedTemperature { get; init; }
}