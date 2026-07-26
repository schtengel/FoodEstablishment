using FluentValidation;
using FoodEstablishment.Api.Modules.Orders.DTOs.Requests;

namespace FoodEstablishment.Api.Modules.Orders.DTOs.Validators;

public class OrderItemCreateRequestValidator : AbstractValidator<OrderItemCreateRequest>
{
    public OrderItemCreateRequestValidator()
    {
        RuleFor(x => x.ProductId)
            .GreaterThan(0).WithMessage("Необходимо указать продукт.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Количество должно быть больше нуля.");
    }
}