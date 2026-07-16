namespace FoodEstablishment.Api.DTOs;

public record VerifyCodeRequest
{
    public string PhoneNumber { get; init; } = string.Empty;
    public string Code { get; init; } = string.Empty;
}