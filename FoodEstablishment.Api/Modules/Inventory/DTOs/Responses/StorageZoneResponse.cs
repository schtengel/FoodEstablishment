namespace FoodEstablishment.Api.Modules.Inventory.DTOs.Responses;

public record StorageZoneResponse
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal RecommendedTemperature { get; init; }
}