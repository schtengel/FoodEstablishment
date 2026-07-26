using FluentValidation;
using FoodEstablishment.Api.DTOs;
using FoodEstablishment.Api.Modules.Identity.DTOs.Requests;

namespace FoodEstablishment.Api.Modules.Identity.DTOs.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    private const string AllowedPasswordCharsPattern = @"^[A-Za-z\d!@#$%^&*()_+\-=]+$";

    public RegisterRequestValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Имя пользователя обязательно.")
            .Length(3, 50).WithMessage("Имя пользователя должно быть от 3 до 50 символов.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Номер телефона обязателен.")
            .Matches(@"^\+7\d{10}$").WithMessage("Номер телефона должен быть в формате +7XXXXXXXXXX.");
        
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Пароль обязателен.")
            .MinimumLength(8).WithMessage("Пароль должен быть не менее 8 символов.")
            .MaximumLength(100).WithMessage("Пароль не должен превышать 100 символов.");
        
        RuleFor(x => x.Password)
            .Cascade(CascadeMode.Stop)
            .Matches(AllowedPasswordCharsPattern)
            .WithMessage("Пароль может содержать только латинские буквы, цифры и символы !@#$%^&*()_+-=.")
            .Matches(@"[A-Z]").WithMessage("Пароль должен содержать хотя бы одну заглавную букву.")
            .Matches(@"[a-z]").WithMessage("Пароль должен содержать хотя бы одну строчную букву.")
            .Matches(@"\d").WithMessage("Пароль должен содержать хотя бы одну цифру.");
    }
}