using FluentValidation;
using FoodEstablishment.Api.Modules.Identity.DTOs.Requests;

namespace FoodEstablishment.Api.Modules.Identity.DTOs.Validators;

public class UserRoleUpdateRequestValidator : AbstractValidator<UserRoleUpdateRequest>
{
    public UserRoleUpdateRequestValidator()
    {
        RuleFor(x => x.Role)
            .IsInEnum().WithMessage("Указана некорректная роль.");
    }
}