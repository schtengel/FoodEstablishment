using FluentValidation;
using FoodEstablishment.Api.Modules.Inventory.DTOs.Requests;

namespace FoodEstablishment.Api.Modules.Inventory.DTOs.Validators;

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