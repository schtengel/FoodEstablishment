using FluentValidation;
using FoodEstablishment.Api.Modules.Orders.DTOs.Requests;

namespace FoodEstablishment.Api.DTOs.Validators;

public class ReceiptStatusUpdateRequestValidator : AbstractValidator<ReceiptStatusUpdateRequest>
{
    public ReceiptStatusUpdateRequestValidator()
    {
        RuleFor(x => x.PaymentStatus)
            .IsInEnum().WithMessage("Указан некорректный статус оплаты.");
    }
}