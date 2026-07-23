using FluentValidation;

namespace FoodEstablishment.Api.DTOs.Validators;

public class TerminalCreateRequestValidator : AbstractValidator<TerminalCreateRequest>
{
    public TerminalCreateRequestValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Название терминала обязательно.")
            .Length(2, 50).WithMessage("Название должно быть от 2 до 50 символов.");

        RuleFor(x => x.DeviceId)
            .NotEmpty().WithMessage("Идентификатор устройства обязателен.")
            .Length(3, 100).WithMessage("Некорректный идентификатор устройства.");
    }
}