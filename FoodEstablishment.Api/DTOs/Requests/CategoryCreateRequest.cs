namespace FoodEstablishment.Api.DTOs;

public record CategoryCreateRequest
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
}