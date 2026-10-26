using FluentAssertions;
using FoodEstablishment.Api.Modules.Identity.Repositories.Interfaces;
using FoodEstablishment.Api.Modules.Menu.Entities;
using FoodEstablishment.Api.Modules.Menu.Repositories.Interfaces;
using FoodEstablishment.Api.Modules.Orders.Controllers;
using FoodEstablishment.Api.Modules.Orders.DTOs.Responses;
using FoodEstablishment.Api.Modules.Orders.Entities;
using FoodEstablishment.Api.Modules.Orders.Enums;
using FoodEstablishment.Api.Modules.Orders.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace FoodEstablishment.Tests.Orders;

public class OrderQueueTests
{
    private readonly IOrderRepository _orders = Substitute.For<IOrderRepository>();
    private readonly OrdersController _controller;

    public OrderQueueTests()
    {
        _controller = new OrdersController(
            _orders,
            Substitute.For<IUserRepository>(),
            Substitute.For<IProductRepository>());
    }

    [Fact]
    public async Task GetQueue_WithoutStatus_AsksForActiveOrders()
    {
        _orders.GetQueueAsync(Arg.Any<IReadOnlyCollection<OrderStatusType>>())
            .Returns(new List<Order>());

        await _controller.GetQueue(null);

        await _orders.Received(1).GetQueueAsync(Arg.Is<IReadOnlyCollection<OrderStatusType>>(statuses =>
            statuses.Count == 3 &&
            statuses.Contains(OrderStatusType.Created) &&
            statuses.Contains(OrderStatusType.InProgress) &&
            statuses.Contains(OrderStatusType.Ready)));
    }

    [Fact]
    public async Task GetQueue_WithStatus_PassesOnlyThatStatus()
    {
        _orders.GetQueueAsync(Arg.Any<IReadOnlyCollection<OrderStatusType>>())
            .Returns(new List<Order>());

        await _controller.GetQueue([OrderStatusType.Ready]);

        await _orders.Received(1).GetQueueAsync(Arg.Is<IReadOnlyCollection<OrderStatusType>>(statuses =>
            statuses.Count == 1 && statuses.Contains(OrderStatusType.Ready)));
    }

    [Fact]
    public async Task GetQueue_ReturnsDishName()
    {
        var order = new Order
        {
            Id = 3,
            UserId = 8,
            OrderSourceId = 1,
            OrderStatusId = (int)OrderStatusType.InProgress,
            CreatedAt = new DateTime(2026, 10, 26, 12, 0, 0, DateTimeKind.Utc),
            OrderCompositions =
            [
                new OrderComposition
                {
                    ProductId = 15,
                    Quantity = 2,
                    PriceAtOrderTime = 100,
                    Product = new Product { Id = 15, Name = "Борщ" },
                },
            ],
        };
        _orders.GetQueueAsync(Arg.Any<IReadOnlyCollection<OrderStatusType>>())
            .Returns(new List<Order> { order });

        var result = await _controller.GetQueue(null);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        var body = ok.Value.Should().BeAssignableTo<IEnumerable<OrderResponse>>().Subject.ToList();
        body.Should().ContainSingle();
        body[0].Items.Should().ContainSingle();
        body[0].Items[0].ProductName.Should().Be("Борщ");
        body[0].Items[0].Quantity.Should().Be(2);
    }
}