using FluentValidation;
using FoodEstablishment.Api.Modules.Orders.DTOs.Requests;

namespace FoodEstablishment.Api.Modules.Orders.DTOs.Validators;

public class OrderCreateRequestValidator : AbstractValidator<OrderCreateRequest>
{
    public OrderCreateRequestValidator()
    {

        RuleFor(x => x.OrderSourceId)
            .GreaterThan(0).WithMessage("Необходимо указать источник заказа.");

        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("Заказ должен содержать хотя бы одну позицию.");

        RuleForEach(x => x.Items)
            .SetValidator(new OrderItemCreateRequestValidator());
        
        RuleFor(x => x.PaymentMethod)
            .IsInEnum().WithMessage("Указан некорректный способ оплаты.");
    }
}