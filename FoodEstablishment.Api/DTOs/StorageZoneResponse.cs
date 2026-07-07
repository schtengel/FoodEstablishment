namespace FoodEstablishment.Api.DTOs;

public class StorageZoneResponse
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal RecommendedTemperature { get; set; }
}