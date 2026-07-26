using FluentValidation;
using FoodEstablishment.Api.DTOs;
using FoodEstablishment.Api.Modules.Identity.DTOs.Requests;

namespace FoodEstablishment.Api.Modules.Identity.DTOs.Validators;

public class GuestSessionRequestValidator : AbstractValidator<GuestSessionRequest>
{
    public  GuestSessionRequestValidator()
    {
        RuleFor(x => x.DeviceId)
            .NotEmpty().WithMessage("Идентификатор устройства обязателен")
            .Length(10, 100).WithMessage("Некорректный идентификатор устройства");
    }
}