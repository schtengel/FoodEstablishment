namespace FoodEstablishment.Api.Modules.Menu.DTOs.Responses;

public record ProductResponse
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public int VolumeOrWeight { get; init; }
    public string Unit { get; init; } = string.Empty;
    public int Calories { get; init; }
    public bool IsStopListed { get; init; }
    public int CategoryId { get; init; }
}