using FluentValidation;

namespace FoodEstablishment.Api.DTOs.Validators;

public class ProductCompositionCreateRequestValidator : AbstractValidator<ProductCompositionCreateRequest>
{
    public  ProductCompositionCreateRequestValidator()
    {
        RuleFor(x => x.ProductId)
            .GreaterThan(0).WithMessage("Необходимо указать продукт.");

        RuleFor(x => x.IngredientId)
            .GreaterThan(0).WithMessage("Необходимо указать ингредиент.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Количество ингредиента должно быть больше нуля.");
    }
}