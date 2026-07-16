using FoodEstablishment.Api.Enums;

namespace FoodEstablishment.Api.DTOs;

public record ProductCreateRequest
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public int VolumeOrWeight { get; init; }
    public UnitType Unit { get; init; }
    public int Calories { get; init; }
    public decimal Price { get; init; }
    public int CategoryId { get; init; }
    public bool IsStopListed { get; init; } = false;
}