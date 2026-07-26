namespace FoodEstablishment.Api.Modules.Identity.DTOs.Requests;

public record VerifyCodeRequest
{
    public string PhoneNumber { get; init; } = string.Empty;
    public string Code { get; init; } = string.Empty;
}