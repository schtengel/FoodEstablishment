using FoodEstablishment.Api.Modules.Identity.Enums;

namespace FoodEstablishment.Api.Modules.Identity.DTOs.Requests;

public record UserRoleUpdateRequest
{
    public UserRoleType Role { get; init; }
}