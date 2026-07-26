using FluentValidation;
using FoodEstablishment.Api.Modules.Orders.DTOs.Requests;

namespace FoodEstablishment.Api.Modules.Orders.DTOs.Validators;

public class ReceiptStatusUpdateRequestValidator : AbstractValidator<ReceiptStatusUpdateRequest>
{
    public ReceiptStatusUpdateRequestValidator()
    {
        RuleFor(x => x.PaymentStatus)
            .IsInEnum().WithMessage("Указан некорректный статус оплаты.");
    }
}