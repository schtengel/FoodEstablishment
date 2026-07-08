using FluentValidation;

namespace FoodEstablishment.Api.DTOs.Validators;

public class SendCodeRequestValidator : AbstractValidator<SendCodeRequest>
{
    public SendCodeRequestValidator()
    {
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Номер телефона обязателен.")
            .Matches(@"^\+7\d{10}$").WithMessage("Номер телефона должен быть в формате +7XXXXXXXXXX.");
    }
}