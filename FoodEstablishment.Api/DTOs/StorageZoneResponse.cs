namespace FoodEstablishment.Api.DTOs;

public class StorageZoneResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal RecommendedTemperature { get; set; }
}