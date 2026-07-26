namespace FoodEstablishment.Api.Modules.Menu.DTOs.Requests;

public record CategoryCreateRequest
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
}