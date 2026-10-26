using FluentAssertions;
using FluentValidation.TestHelper;
using FoodEstablishment.Api.Modules.Identity.Repositories.Interfaces;
using FoodEstablishment.Api.Modules.Inventory.DTOs.Requests;
using FoodEstablishment.Api.Modules.Inventory.DTOs.Validators;
using FoodEstablishment.Api.Modules.Inventory.Entities;
using FoodEstablishment.Api.Modules.Menu.Entities;
using FoodEstablishment.Api.Modules.Menu.Repositories.Interfaces;
using FoodEstablishment.Api.Modules.Orders.Controllers;
using FoodEstablishment.Api.Modules.Orders.Entities;
using FoodEstablishment.Api.Modules.Orders.Repositories.Interfaces;
using FoodEstablishment.Api.Modules.Orders.Services;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace FoodEstablishment.Tests.Orders;

public class StockDeductionTests
{
    [Fact]
    public void Plan_SameIngredientOnTwoDishes_SumsQuantity()
    {
        var flour = new Ingredient { Id = 1, Name = "Мука", StockQuantity = 7 };
        var lines = new List<OrderComposition>
        {
            Line(productId: 10, portions: 2, new ProductComposition { IngredientId = 1, Quantity = 3 }),
            Line(productId: 11, portions: 1, new ProductComposition { IngredientId = 1, Quantity = 1 }),
        };

        var plan = StockDeduction.Plan(lines, new Dictionary<int, Ingredient> { [1] = flour });

        var ready = plan.Should().BeOfType<StockDeductionPlan.Ready>().Subject;
        ready.Lines.Should().ContainSingle().Which.Should().Be(new StockDeductionLine(1, 7));
        flour.StockQuantity.Should().Be(7);
    }

    [Fact]
    public void Apply_WhenStockIsShort_GoesNegative()
    {
        var flour = new Ingredient { Id = 1, Name = "Мука", StockQuantity = 5 };
        var ingredients = new Dictionary<int, Ingredient> { [1] = flour };
        var plan = StockDeduction.Plan(
            [Line(10, 2, new ProductComposition { IngredientId = 1, Quantity = 3 })],
            ingredients);

        var ready = plan.Should().BeOfType<StockDeductionPlan.Ready>().Subject;
        StockDeduction.Apply(ready, ingredients);

        flour.StockQuantity.Should().Be(-1);
    }

    [Fact]
    public void Plan_DeletedIngredient_IsRejected()
    {
        var flour = new Ingredient { Id = 4, Name = "Мука", StockQuantity = 100, IsDeleted = true };

        var plan = StockDeduction.Plan(
            [Line(10, 1, new ProductComposition { IngredientId = 4, Quantity = 1 })],
            new Dictionary<int, Ingredient> { [4] = flour });

        plan.Should().BeOfType<StockDeductionPlan.MissingIngredient>()
            .Which.IngredientId.Should().Be(4);
    }

    private static OrderComposition Line(int productId, int portions, params ProductComposition[] parts) => new()
    {
        ProductId = productId,
        Quantity = portions,
        Product = new Product
        {
            Id = productId,
            ProductCompositions = parts.ToList(),
        },
    };
}

public class MarkGivenControllerTests
{
    private readonly IOrderRepository _orders = Substitute.For<IOrderRepository>();
    private readonly OrdersController _controller;

    public MarkGivenControllerTests()
    {
        _controller = new OrdersController(
            _orders,
            Substitute.For<IUserRepository>(),
            Substitute.For<IProductRepository>());
    }

    [Fact]
    public async Task MarkGiven_Success_ReturnsNoContent()
    {
        _orders.TryMarkGivenAsync(7).Returns(new MarkGivenResult.Success());

        var result = await _controller.MarkGiven(7);

        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task MarkGiven_MissingOrder_ReturnsNotFound()
    {
        _orders.TryMarkGivenAsync(7).Returns(new MarkGivenResult.NotFound());

        var result = await _controller.MarkGiven(7);

        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task MarkGiven_Rejected_ReturnsBadRequest()
    {
        _orders.TryMarkGivenAsync(7).Returns(
            new MarkGivenResult.Rejected("Ингредиент с Id = 4 отсутствует на складе. Выдать заказ нельзя."));

        var result = await _controller.MarkGiven(7);

        var badRequest = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        badRequest.Value.Should().Be("Ингредиент с Id = 4 отсутствует на складе. Выдать заказ нельзя.");
    }
}

public class IngredientStockEditTests
{
    private readonly IngredientCreateRequestValidator _validator = new();

    [Fact]
    public void Update_NegativeStock_IsAllowed()
    {
        var result = _validator.TestValidate(new IngredientCreateRequest
        {
            Name = "Мука",
            Unit = "кг",
            StockQuantity = -1.5m,
            StorageZoneId = 1,
        });

        result.ShouldNotHaveAnyValidationErrors();
    }
}