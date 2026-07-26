using FluentValidation;
using FoodEstablishment.Api.Modules.Inventory.DTOs.Requests;

namespace FoodEstablishment.Api.Modules.Inventory.DTOs.Validators;

public class IngredientCreateRequestValidator : AbstractValidator<IngredientCreateRequest>
{
    public IngredientCreateRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Название ингредиента обязательно.")
            .Length(2, 100).WithMessage("Название должно быть от 2 до 100 символов.");

        RuleFor(x => x.Unit)
            .NotEmpty().WithMessage("Единица измерения обязательна.")
            .MaximumLength(10).WithMessage("Единица измерения не должна превышать 10 символов.");

        RuleFor(x => x.StockQuantity)
            .GreaterThanOrEqualTo(0).WithMessage("Количество на складе не может быть отрицательным.");

        RuleFor(x => x.StorageZoneId)
            .GreaterThan(0).WithMessage("Необходимо указать зону хранения.");
    }
}