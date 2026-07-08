using FluentValidation;

namespace FoodEstablishment.Api.DTOs.Validators;

public class VerifyCodeRequestValidator : AbstractValidator<VerifyCodeRequest>
{
    public VerifyCodeRequestValidator()
    {
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Номер телефона обязателен.")
            .Matches(@"^\+7\d{10}$").WithMessage("Номер телефона должен быть в формате +7XXXXXXXXXX.");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Код подтверждения обязателен.")
            .Length(4).WithMessage("Код должен состоять из 4 цифр.")
            .Matches(@"^\d{4}$").WithMessage("Код должен состоять только из цифр.");
    }
}