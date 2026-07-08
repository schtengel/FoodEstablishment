using FluentValidation;

namespace FoodEstablishment.Api.DTOs.Validators;

public class StorageZoneCreateRequestValidator : AbstractValidator<StorageZoneCreateRequest>
{
    public StorageZoneCreateRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Название зоны хранения обязательно.")
            .Length(3, 50).WithMessage("Название должно быть от 3 до 50 символов.");

        RuleFor(x => x.RecommendedTemperature)
            .InclusiveBetween(-23, 25).WithMessage("Температура должна быть в диапазоне от -23 до 25 градусов.");
    }
}