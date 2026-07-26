using FluentValidation;
using FoodEstablishment.Api.DTOs;
using FoodEstablishment.Api.Modules.Menu.DTOs.Requests;

namespace FoodEstablishment.Api.Modules.Menu.DTOs.Validators;

public class ProductCreateRequestValidator : AbstractValidator<ProductCreateRequest>
{
    public  ProductCreateRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Название продукта обязательно.")
            .Length(2, 100).WithMessage("Название должно быть от 2 до 100 символов.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Описание не должно превышать 500 символов.");
        
        RuleFor(x => x.VolumeOrWeight)
            .InclusiveBetween(1, 10000).WithMessage("Вес или объем должен быть от 1 до 10 000.");

        RuleFor(x => x.Unit)
            .IsInEnum().WithMessage("Указана неверная единица измерения");

        RuleFor(x => x.Calories)
            .GreaterThan(0).WithMessage("Калорийность должна быть положительной.");
        
        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Цена должна быть больше нуля.");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0).WithMessage("Необходимо указать корректную категорию.");
    }
}