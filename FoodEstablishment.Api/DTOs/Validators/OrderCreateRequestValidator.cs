using FluentValidation;

namespace FoodEstablishment.Api.DTOs.Validators;

public class OrderCreateRequestValidator : AbstractValidator<OrderCreateRequest>
{
    public OrderCreateRequestValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage("Необходимо указать пользователя.");

        RuleFor(x => x.OrderSourceId)
            .GreaterThan(0).WithMessage("Необходимо указать источник заказа.");

        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("Заказ должен содержать хотя бы одну позицию.");

        RuleForEach(x => x.Items)
            .SetValidator(new OrderItemCreateRequestValidator());
    }
}