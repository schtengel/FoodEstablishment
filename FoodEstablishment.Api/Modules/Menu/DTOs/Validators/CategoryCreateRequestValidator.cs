using FluentValidation;
using FoodEstablishment.Api.DTOs;
using FoodEstablishment.Api.Modules.Menu.DTOs.Requests;

namespace FoodEstablishment.Api.Modules.Menu.DTOs.Validators;

public class CategoryCreateRequestValidator : AbstractValidator<CategoryCreateRequest>
{
    public CategoryCreateRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Название категории обязательно для заполнения.")
            .Length(3, 50).WithMessage("Название должно быть от 3 до 50 символов.");

        RuleFor(x => x.Description)
            .MaximumLength(200).WithMessage("Описание не должно превышать 200 символов.");
    }
}